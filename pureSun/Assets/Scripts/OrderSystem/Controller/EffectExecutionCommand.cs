

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Util;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using RiderEditor;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class EffectExecutionCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            EffectInfoProxy effectInfoProxy =
                Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            PlayerGroupProxy playerGroupProxy =
                Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            OperateSystemProxy operateSystemProxy =
                Facade.RetrieveProxy(OperateSystemProxy.NAME) as OperateSystemProxy;
            QuestStageCircuitProxy questStageCircuitProxy =
                Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            MinionGridProxy minionGridProxy =
                Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;
            //待执行的卡牌
            CardEntry exeEffectCard = null;
            //提取待执行的卡牌效果
            List<EffectInfo> effectInfos = new List<EffectInfo>();
            switch (notification.Type)
            {
                //执行一张卡
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD:
                    exeEffectCard = notification.Body as CardEntry;
                    //遍历卡牌的效果
                    foreach (string effectCode in exeEffectCard.effectCodeList)
                    {
                        EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                        //设置状态
                        oneEffectInfo.effectInfoStage = EffectInfoStage.UnStart;
                        //设置所有者,手牌操作模式，所有者是当前玩家
                        oneEffectInfo.player = exeEffectCard.player;
                        //设置所属卡牌
                        oneEffectInfo.cardEntry = exeEffectCard;


                        //是否有预先选定的目标对象
                        //将预先选择的目标存入效果中
                        if (effectCode == exeEffectCard.cardInfo.targetEffectInfo)
                        {
                            foreach (TargetSet targetSet in oneEffectInfo.targetSetList) {
                                if (targetSet.target == "Minion")
                                {
                                    oneEffectInfo.targetSetList[0].targetMinionCellItems.Add(exeEffectCard.targetMinionCellItem);
                                }
                            }
                        }
                        effectInfos.Add(oneEffectInfo);
                    }
                    //存入效果，进行结算
                    effectInfoProxy.IntoModeCardSettle(exeEffectCard, effectInfos);
                    break;
                //执行一张已经触发效果的卡
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_TRIGGERED_CARD:
                    exeEffectCard = notification.Body as CardEntry;

                    effectInfos.Add(exeEffectCard.triggeredEffectInfo);
                    effectInfoProxy.IntoModeCardSettle(exeEffectCard, effectInfos);
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_TARGET:
                    //发送打开效果展示列表的消息
                    SendNotification(
                         UIViewSystemEvent.UI_VIEW_CURRENT,
                         null,
                         StringUtil.GetNTByNotificationTypeAndUIViewName(
                             UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                             UIViewConfig.getNameStrByUIViewName(UIViewName.EffectDisplayView)
                             )
                         );

                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        UtilityLog.Log("效果【" + effect.description + "】开始寻找目标",LogUtType.Effect);
                        if (effect.effectInfoStage == EffectInfoStage.UnStart)
                        {
                            ExecutionEffectFindTarget(effect, playerGroupProxy, questStageCircuitProxy, minionGridProxy);
                            //插入了用户操作
                            if (effect.effectInfoStage == EffectInfoStage.Confirming)
                            {
                                break;
                            }
                        }
                        //如果是正在选择，逻辑上不存在这种情况
                        else if (effect.effectInfoStage == EffectInfoStage.Confirming)
                        {
                            UtilityLog.LogError("Should not exist Confirming EffectInfoStage");
                        }
                    }
                    //判断是否所有效果都存在了目标
                    bool allEffectHasTarget = true;
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        if (effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage != EffectInfoStage.ConfirmedTarget)
                        {
                            allEffectHasTarget = false;
                        }
                    }
                    //开始执行效果
                    if (allEffectHasTarget)
                    {
                        SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_EFFECT);
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EFFECT_SHOW_OVER:
                    effectInfoProxy.effectSysItem.showEffectNum--;
                    if (effectInfoProxy.effectSysItem.effectSysItemStage == EffectSysItemStage.UnStart)
                    {
                        if (effectInfoProxy.effectSysItem.cardEntryQueue.Count == 0)
                        {
                            if (effectInfoProxy.effectSysItem.showEffectNum == 0) {
                                UtilityLog.Log("所有效果展示完毕" + System.Guid.NewGuid().ToString("N"), LogUtType.Effect);
                                //通知回合控制器当前堆叠已经全部执行完毕
                                SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE);
                            }
                          
                        }
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_MINION_BUFF_NEED_REMOVE:
                    MinionCellItem needRemoveBuffMinionCellItem = notification.Body as MinionCellItem;
                    UtilityLog.Log("生物【" + needRemoveBuffMinionCellItem.cardEntry.name + "】的buff需要被清除", LogUtType.Effect);
                    foreach (EffectInfo oneEffectInfoBuffCheck in needRemoveBuffMinionCellItem.effectBuffInfoList)
                    {
                        //倒计时小于则清除buff
                        if (oneEffectInfoBuffCheck.effectiveTime.ContinuousRound < 0)
                        {
                            EffectInfo oneReverseEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(oneEffectInfoBuffCheck.code);

                            oneReverseEffectInfo.targetSetList[0].targetMinionCellItems.Add(needRemoveBuffMinionCellItem);
                            oneReverseEffectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                            oneReverseEffectInfo.whetherToshow = "N";
                            oneReverseEffectInfo.isReverse = true;

                            CardEntry cardEntry = new CardEntry();
                            cardEntry.player = playerGroupProxy.getPlayerByPlayerCode(needRemoveBuffMinionCellItem.playerCode);
                            oneReverseEffectInfo.cardEntry = cardEntry;
                            effectInfos.Add(oneReverseEffectInfo);
                            effectInfoProxy.IntoModeCardSettle(exeEffectCard, effectInfos);
                        }
                        
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_EFFECT:
                    ExecutionEffectContent(effectInfoProxy);
                    //判断是否所有效果都执行完毕了
                    bool allEffectHasFinished = true;
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        if (effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage != EffectInfoStage.Finished)
                        {
                            allEffectHasFinished = false;
                        }
                    }
                    if (allEffectHasFinished)
                    {
                        //判断这一组效果是不是都是一张卡的，逻辑上来说都是一张卡
                        CardEntry cardCheck = new CardEntry();
                        bool cardCheckOver = true;
                        for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                        {
                            if (n == 0)
                            {
                                cardCheck = effectInfoProxy.effectSysItem.effectInfos[n].cardEntry;
                            }
                            else
                            {
                                if (effectInfoProxy.effectSysItem.effectInfos[n].cardEntry.uuid != cardCheck.uuid)
                                {
                                    cardCheckOver = false;
                                    break;
                                }
                                else
                                {
                                    cardCheck = effectInfoProxy.effectSysItem.effectInfos[n].cardEntry;
                                }
                            }
                        }
                        if (!cardCheckOver)
                        {
                            UtilityLog.LogError("同一个效果组来源多余一个实体");
                        }
                        else {
                            if (operateSystemProxy.operateSystemItem.operateModeType == OperateSystemItem.OperateType.HandUse) {
                                //判断来源,如果是正在使用的手牌
                                if (operateSystemProxy.operateSystemItem.onChooseHandCellItem.cardEntry.uuid == cardCheck.uuid)
                                {
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE_EXE_OVER);
                                }
                            }
                        }
                        //执行下一组效果
                        effectInfoProxy.effectSysItem.effectSysItemStage = EffectSysItemStage.UnStart;
                        //通知时点触发器这一组效果执行完毕了
                        SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, null, TimeTriggerEvent.TIME_TRIGGER_EXE_NEXT_DELAY_NOTIFICATION);
                        effectInfoProxy.ExeEffectQueue();
                    }
                    else
                    {
                        UtilityLog.LogError("效果执行失败");
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_LAUNCH_AN_ATTACK:
                    MinionCellItem attackMinionCellItem = notification.Body as MinionCellItem;
                    PlayerItem playerItemAttack = playerGroupProxy.getPlayerByPlayerCode(attackMinionCellItem.playerCode);
                    HexCoordinates vectorHexCoordinates = new HexCoordinates(playerItemAttack.playerSiteOne.attackDefaultDirection.x, playerItemAttack.playerSiteOne.attackDefaultDirection.z);
                    HexCoordinates targetHexCoordinates = HexUtil.GetTargetHexCoordinatesByStartPointAndVector(attackMinionCellItem.index,vectorHexCoordinates);
                    //判断目标单元格上有没有生物
                    if (minionGridProxy.minionGridItem.minionCells.ContainsKey(targetHexCoordinates)) {
                        //如果有生物，需要再判断是自己的生物还是对手的生物
                        MinionCellItem defensiveMinionCellItem = minionGridProxy.minionGridItem.minionCells[targetHexCoordinates];
                        if (defensiveMinionCellItem.playerCode != attackMinionCellItem.playerCode) {
                            UtilityLog.Log("【" + attackMinionCellItem.cardEntry.name + "】进行攻击", LogUtType.Attack);
                            //攻击
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            attackMinionCellItem.AttackTargetMinion(defensiveMinionCellItem);
                            //反击
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            defensiveMinionCellItem.CounterAttackTargetMinion(attackMinionCellItem);
                        }
                    }

                    break;
            }
        }
        //执行效果
        public void ExecutionEffectContent(EffectInfoProxy effectInfoProxy)
        {
            for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
            {
                EffectInfo effectInfo = effectInfoProxy.effectSysItem.effectInfos[n];
                effectInfo.effectInfoStage = EffectInfoStage.Executing;
                if (effectInfo.targetSetList.Count == 1)
                {
                    switch (effectInfo.targetSetList[0].target)
                    {
                        //效果选择
                        case "ChooseEffect":
                            //无需执行，玩家自己操作

                            break;
                        //玩家
                        case "Player":
                            effectInfo.TargetPlayerList(effectInfo.targetSetList[0].targetPlayerItems);
                            break;
                        case "Minion":
                            effectInfo.TargetMinionList(effectInfo.targetSetList[0].targetMinionCellItems);
                            break;

                    }
                    effectInfo.effectInfoStage = EffectInfoStage.Finished;
                    //选择效果无需展示
                    if (effectInfo.whetherToshow == "Y")
                    {
                        //发送已经确认目标的效果到前台进行展示
                        CardEntry oneCardEntry = effectInfo.cardEntry;
                        oneCardEntry.needShowEffectInfo = effectInfo;
                        effectInfoProxy.effectSysItem.showEffectNum++;
                        SendNotification(UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS, oneCardEntry, UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_PUT_ONE_EFFECT);
                    }
                }

            }
        }


        //为效果选择目标
        public void ExecutionEffectFindTarget(  EffectInfo effectInfo, 
                                                PlayerGroupProxy playerGroupProxy, 
                                                QuestStageCircuitProxy questStageCircuitProxy,
                                                MinionGridProxy minionGridProxy)
        {
            effectInfo.effectInfoStage = EffectInfoStage.Confirming;
            foreach (TargetSet targetSet in effectInfo.targetSetList)
            {
                bool designationTargetOver = false;
                //先判断是否已经指定了目标
                switch (targetSet.target)
                {
                    case "Minion":
                        if (targetSet.targetMinionCellItems.Count == targetSet.targetClaimsNums) {
                            designationTargetOver = true;
                        }
                        break;
                }
                if (designationTargetOver)
                {
                    effectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                    continue;
                }


                //条件
                string[] targetClaims = targetSet.targetClaims;
                //条件内容
                string[] targetClaimsContents = targetSet.targetClaimsContents;
                //目标玩家
                PlayerItem targetPlayer = null;
                //类型
                switch (targetSet.target)
                {
                    case "Minion":
                        List<MinionCellItem> minionCellItems = new List<MinionCellItem>();
                        foreach (MinionCellItem minionCellItem in minionGridProxy.GetMinionCellItemListByPlayerCode(playerGroupProxy.getPlayerByPlayerCode(questStageCircuitProxy.GetNowPlayerCode())))
                        {
                            for (int n = 0; n < targetClaims.Length; n++)
                            {
                                //判断所有权
                                if (targetClaims[n] == "Owner")
                                {
                                    //是自己选
                                    if (targetClaimsContents[n] == "Myself")
                                    {
                                        if (minionCellItem.playerCode == effectInfo.player.playerCode)
                                        {
                                            minionCellItems.Add(minionCellItem);
                                        }
                                    }
                                }
                            }
                        }
                        if (minionCellItems.Count < targetSet.targetClaimsNums)
                        {
                            //符合数量限制
                            targetSet.hasTarget = true;
                            targetSet.targetMinionCellItems = minionCellItems;
                        }
                        else {
                            //超出目标上限，需要用户选择
                            UtilityLog.LogError("超出目标上限");
                        }
                        
                        break;
                    //效果选择
                    case "ChooseEffect":
                        //获取玩家，根据条件筛选出复合条件的释放者和选择者
                        //筛选结果
                        foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                        {
                            for (int n = 0; n < targetClaims.Length; n++)
                            {
                                //判断所有权
                                if (targetClaims[n] == "Owner")
                                {
                                    //是自己选
                                    if (targetClaimsContents[n] == "Myself")
                                    {
                                        if (playerItem.playerCode == effectInfo.player.playerCode)
                                        {
                                            targetPlayer = playerItem;
                                        }
                                    }
                                }
                            }
                        }
                        if (targetPlayer != null)
                        {
                            effectInfo.chooseByPlayer = targetPlayer;
                            //发布用户需要选择信号
                            SendNotification(LogicalSysEvent.LOGICAL_SYS, effectInfo, LogicalSysEvent.LOGICAL_SYS_CHOOSE_EFFECT);
                            //
                        }
                        else
                        {
                            UtilityLog.LogError("no player can ChooseEffect");
                        }

                        break;
                    //玩家
                    case "Player":
                        //获取玩家，根据条件筛选出复合条件的释放者和选择者
                        //筛选结果
                        foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                        {
                            for (int n = 0; n < targetClaims.Length; n++)
                            {
                                //判断所有权
                                if (targetClaims[n] == "Owner")
                                {
                                    //是自己选
                                    if (targetClaimsContents[n] == "Myself")
                                    {
                                        if (playerItem.playerCode == effectInfo.player.playerCode)
                                        {
                                            targetPlayer = playerItem;
                                        }
                                    }
                                }
                            }
                        }
                        if (targetPlayer != null)
                        {
                            //玩家确认
                            targetSet.targetPlayerItems.Add(targetPlayer);
                            targetSet.hasTarget = true;
                        }
                        else
                        {
                            UtilityLog.LogError("no player can add TargetPlayerItems");
                        }
                        break;

                }
            }
            bool hasTargetOver = true;
            foreach (TargetSet targetSet in effectInfo.targetSetList)
            {
                if (!targetSet.hasTarget) {
                    hasTargetOver = false;
                }
            }
            if (hasTargetOver) {
                UtilityLog.Log("【" + effectInfo.description + "】目标确认完毕",LogUtType.Effect);
                effectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
            }


        }
    }
}
