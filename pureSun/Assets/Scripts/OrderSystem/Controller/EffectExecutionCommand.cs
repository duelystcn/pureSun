

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
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
                    foreach (string effectName in exeEffectCard.effectName)
                    {
                        EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectName);
                        //设置状态
                        oneEffectInfo.effectInfoStage = EffectInfoStage.UnStart;
                        //设置所有者,手牌操作模式，所有者是当前玩家
                        oneEffectInfo.player = exeEffectCard.player;
                        //设置所属卡牌
                        oneEffectInfo.cardEntry = exeEffectCard;

                        //是否有预先选定的目标对象
                        //将预先选择的目标存入效果中
                        if (effectName == exeEffectCard.cardInfo.targetEffectInfo)
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
                        if (effect.effectInfoStage == EffectInfoStage.UnStart)
                        {
                          
                            ExecutionEffectFindTarget(effect, playerGroupProxy);
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
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_EFFECT:
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        ExecutionEffectContent(effectInfoProxy.effectSysItem.effectInfos[n]);

                    }
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
                        //判断状态
                        switch (operateSystemProxy.operateSystemItem.operateModeType)
                        {
                            //手牌使用状态
                            case OperateSystemItem.OperateType.HandUse:
                                //返回手牌使用完毕的信息，移除手牌
                                playerGroupProxy.getPlayerByPlayerCode(operateSystemProxy.operateSystemItem.playerItem.playerCode).RemoveOneCardByUse(operateSystemProxy.operateSystemItem.onChooseHandCellItem);
                                //结束，改变模式为初始，清除手牌
                                operateSystemProxy.IntoModeClose();
                                break;
                        }
                        //执行下一组效果
                        effectInfoProxy.effectSysItem.effectSysItemStage = EffectSysItemStage.UnStart;
                        effectInfoProxy.ExeEffectQueue();
                    }
                    else
                    {
                        UtilityLog.LogError("效果执行失败");
                    }
                    break;
            }
        }
        //执行效果
        public void ExecutionEffectContent(EffectInfo effectInfo)
        {
            effectInfo.effectInfoStage = EffectInfoStage.Executing;
            if (effectInfo.targetSetList.Count == 1) {
                switch (effectInfo.targetSetList[0].target)
                {
                    //效果选择
                    case "ChooseEffect":
                        //无需执行，玩家自己操作

                        break;
                    //玩家
                    case "Player":
                        UtilityLog.Log("effectInfo:" + effectInfo.description);
                        effectInfo.TargetPlayerList(effectInfo.targetSetList[0].targetPlayerItems);
                        break;

                }
                effectInfo.effectInfoStage = EffectInfoStage.Finished;
                //选择效果无需展示
                if (effectInfo.targetSetList[0].target != "ChooseEffect")
                {
                    //发送已经确认目标的效果到前台进行展示
                    CardEntry oneCardEntry = effectInfo.cardEntry;
                    oneCardEntry.needShowEffectInfo = effectInfo;
                    SendNotification(UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS, oneCardEntry, UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_PUT_ONE_EFFECT);
                }
            }
           


        }


        //为效果选择目标
        public void ExecutionEffectFindTarget(EffectInfo effectInfo, PlayerGroupProxy playerGroupProxy)
        {
            effectInfo.effectInfoStage = EffectInfoStage.Confirming;
            foreach (TargetSet targetSet in effectInfo.targetSetList)
            {
                //条件
                string[] targetClaims = targetSet.targetClaims;
                //条件内容
                string[] targetClaimsContents = targetSet.targetClaimsContents;
                //目标玩家
                PlayerItem targetPlayer = null;
                //类型
                switch (targetSet.target)
                {
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
                            UtilityLog.Log("no player can ChooseEffect");
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
                            UtilityLog.Log("no player can add TargetPlayerItems");
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
                effectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
            }


        }
    }
}
