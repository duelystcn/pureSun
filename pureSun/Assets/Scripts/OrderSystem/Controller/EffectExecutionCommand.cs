

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.EffectCompent;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Util;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using RiderEditor;
using System;
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
            GameContainerProxy gameContainerProxy =
              Facade.RetrieveProxy(GameContainerProxy.NAME) as GameContainerProxy;

            //待执行的卡牌
            CardEntry exeEffectCard = null;
           
            switch (notification.Type)
            {
                //执行一张卡
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD:
                    exeEffectCard = notification.Body as CardEntry;
                    operateSystemProxy.IntoModeByType(exeEffectCard,exeEffectCard.controllerPlayerItem, OperateSystemItem.OperateType.CardIsReleasing);
                    //提取待执行的卡牌效果
                    List<EffectInfo> oneExeEffectCardEffects = new List<EffectInfo>();
                    //遍历卡牌的效果
                    foreach (string effectCode in exeEffectCard.effectCodeList)
                    {
                        EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                        if (oneEffectInfo.impactTimeTriggers == null) {
                            //设置状态
                            oneEffectInfo.effectInfoStage = EffectInfoStage.UnStart;
                            //设置所有者,手牌操作模式，所有者是当前玩家
                            oneEffectInfo.player = exeEffectCard.controllerPlayerItem;
                            //设置所属卡牌
                            oneEffectInfo.cardEntry = exeEffectCard;
                            //是否有预先选定的目标对象
                            if (exeEffectCard.targetBasicGameDto != null)
                            {
                                if (exeEffectCard.targetBasicGameDto.dtoType == "Minion")
                                {
                                    foreach (TargetSet targetSet in oneEffectInfo.operationalTarget.selectTargetList)
                                    {
                                        if (targetSet.target == "Minion")
                                        {
                                            targetSet.targetMinionCellItems.Add(exeEffectCard.targetBasicGameDto as CardEntry);
                                        }
                                    }
                                }
                            }
                            oneExeEffectCardEffects.Add(oneEffectInfo);
                        }
                    }
                    //存入效果，进行结算
                    effectInfoProxy.IntoModeCardSettle(exeEffectCard, oneExeEffectCardEffects);
                    break;
                //执行一张已经触发效果的卡
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_TRIGGERED_CARD:
                    exeEffectCard = notification.Body as CardEntry;
                    //提取待执行的卡牌效果
                    List<EffectInfo> oneTriggeredEffectCardEffects = new List<EffectInfo>();
                    oneTriggeredEffectCardEffects.Add(exeEffectCard.triggeredEffectInfo);
                    effectInfoProxy.IntoModeCardSettle(exeEffectCard, oneTriggeredEffectCardEffects);
                    break; 
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_TARGET:
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        if (effect.effectType == "Independent")
                        {
                            bool hasTarget =  ExecutionEffectFindTarget(effect, effectInfoProxy, playerGroupProxy, questStageCircuitProxy, gameContainerProxy);
                            //没能正确的寻找到目标，防止循环继续执行下去导致效果执行两边，直接返回
                            if (!hasTarget) {
                                return;
                            }
                           
                        }
                        else if (effect.effectType == "BeforeAndAfter")
                        {
                            bool allHasTarget = ExecutionEffectFindTargetForBeforeAndAfter(effect, playerGroupProxy, questStageCircuitProxy, effectInfoProxy, gameContainerProxy);
                            if (!allHasTarget)
                            {
                                return;
                            }
                        }
                    }
                    //判断是否所有效果都存在了目标
                    bool allEffectHasTarget = true;
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        if (effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage == EffectInfoStage.UnStart||
                            effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage == EffectInfoStage.ConfirmingTarget)
                        {
                            allEffectHasTarget = false;
                        }
                    }
                    //询问是否需要用户操作
                    if (allEffectHasTarget)
                    {
                        SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_ASK_THE_USER);
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_ASK_THE_USER:
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        if (effect.effectInfoStage == EffectInfoStage.ConfirmedTarget)
                        {
                            AskTheUserOperating(effect,
                                             playerGroupProxy,
                                             questStageCircuitProxy,
                                             effectInfoProxy,
                                             gameContainerProxy
                                             );

                        }
                    }
                    //判断是否所有效果都进行了用户选择
                    bool allEffectHasUserSelect = true;
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        if (effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage == EffectInfoStage.UnStart
                              || effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage == EffectInfoStage.ConfirmingTarget
                              || effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage == EffectInfoStage.ConfirmedTarget
                              || effectInfoProxy.effectSysItem.effectInfos[n].effectInfoStage == EffectInfoStage.AskTheUser
                            )
                        {
                            allEffectHasUserSelect = false;
                        }
                    }
                    if (allEffectHasUserSelect)
                    {
                        //执行效果
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
                    CardEntry needRemoveBuffMinionCellItem = notification.Body as CardEntry;
                    UtilityLog.Log("生物【" + needRemoveBuffMinionCellItem.name + "】的buff需要被清除", LogUtType.Effect);
                    foreach (EffectInfo oneEffectInfoBuffCheck in needRemoveBuffMinionCellItem.effectBuffInfoList)
                    {
                        if (oneEffectInfoBuffCheck.effectiveTime.ContinuousRound == 0)
                        {
                            //提取待执行的卡牌效果
                            List<EffectInfo> oneBuffCleanEffectCardEffects = new List<EffectInfo>();
                            UtilityLog.Log("生物【" + needRemoveBuffMinionCellItem.name + "】的buff【" + oneEffectInfoBuffCheck.code  + "】需要被清除", LogUtType.Special);
                            EffectInfo oneReverseEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(oneEffectInfoBuffCheck.code);
                            foreach (TargetSet targetSet in oneReverseEffectInfo.operationalTarget.selectTargetList)
                            {
                                targetSet.targetMinionCellItems.Add(needRemoveBuffMinionCellItem);
                            }
                            oneReverseEffectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                            oneReverseEffectInfo.whetherToshow = "N";
                            oneReverseEffectInfo.isReverse = true;

                            CardEntry cardEntry = new CardEntry();
                            cardEntry.name = "BuffClean";
                            cardEntry.controllerPlayerItem = needRemoveBuffMinionCellItem.controllerPlayerItem;
                            oneReverseEffectInfo.cardEntry = cardEntry;
                            oneBuffCleanEffectCardEffects.Add(oneReverseEffectInfo);
                            effectInfoProxy.IntoModeCardSettle(cardEntry, oneBuffCleanEffectCardEffects);
                        }
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_EFFECT:
                    ExecutionEffectContentList(effectInfoProxy, gameContainerProxy);
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
                        for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                        {
                            if (effectInfoProxy.effectSysItem.effectInfos[n].impactTimeTriggertMonitorListWhenOver != null)
                            {
                                //把效果结算完毕的时点都发送出去
                                foreach (string impactTimeTriggertMonitor in effectInfoProxy.effectSysItem.effectInfos[n].impactTimeTriggertMonitorListWhenOver)
                                {
                                   
                                    SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(impactTimeTriggertMonitor, effectInfoProxy.effectSysItem.effectInfos[n].player.playerCode));
                                }
                            }
                        }
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
                        if (effectInfoProxy.effectSysItem.effectInfos.Count == 0) {
                            cardCheck = effectInfoProxy.effectSysItem.cardEntry;
                        }
                        if (!cardCheckOver)
                        {
                            UtilityLog.LogError("同一个效果组来源多余一个实体");
                        }
                        else {
                            if (cardCheck.lastGameContainerType == "CardHand") {
                                SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE_EXE_OVER);
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
                    CardEntry attackMinionCellItem = notification.Body as CardEntry;
                    PlayerItem playerItemAttack = attackMinionCellItem.controllerPlayerItem;
                    HexCoordinates vectorHexCoordinates = new HexCoordinates(playerItemAttack.playerSiteOne.attackDefaultDirection.x, playerItemAttack.playerSiteOne.attackDefaultDirection.z);
                    HexCoordinates targetHexCoordinates = HexUtil.GetTargetHexCoordinatesByStartPointAndVector(attackMinionCellItem.nowIndex,vectorHexCoordinates);

                    CardEntry defensiveMinionCellItem = gameContainerProxy.CheckHasCardEntryByGameContainerTypeAndHexCoordinates("CardBattlefield", targetHexCoordinates);

                    //判断目标单元格上有没有生物
                    if (defensiveMinionCellItem != null) {
                        //如果有生物，需要再判断是自己的生物还是对手的生物
                        if (defensiveMinionCellItem.controllerPlayerItem != attackMinionCellItem.controllerPlayerItem) {
                            UtilityLog.Log("【" + attackMinionCellItem.name + "】进行攻击", LogUtType.Attack);
                            //攻击
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            attackMinionCellItem.AttackTargetMinion(defensiveMinionCellItem);
                            //反击
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            defensiveMinionCellItem.CounterAttackTargetMinion(attackMinionCellItem);
                        }
                    }
                    break;
                case EffectExecutionEvent.EFFECT_EXECUTION_SYS_LAUNCH_AN_MOVE:
                    CardEntry moveMinionCellItem = notification.Body as CardEntry;
                    PlayerItem playerItemMove = moveMinionCellItem.controllerPlayerItem;
                    HexCoordinates vectorHexCoordinatesMove = new HexCoordinates(playerItemMove.playerSiteOne.attackDefaultDirection.x, playerItemMove.playerSiteOne.attackDefaultDirection.z);
                    HexCoordinates targetMoveHexCoordinates = HexUtil.GetTargetHexCoordinatesByStartPointAndVector(moveMinionCellItem.nowIndex, vectorHexCoordinatesMove);
                    //判断目标单元格是否在可移动范围内
                    if (playerItemMove.CheckOneHexCanMove(targetMoveHexCoordinates)) {
                        CardEntry moveHexCoordinatesCard = gameContainerProxy.CheckHasCardEntryByGameContainerTypeAndHexCoordinates("CardBattlefield", targetMoveHexCoordinates);
                        //判断目标单元格上有没有生物,没有生物才能移动
                        if (moveHexCoordinatesCard == null)
                        {
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            moveMinionCellItem.MoveToTargetHexCoordinates(targetMoveHexCoordinates);
                        }
                    }
                    break;
                //case EffectExecutionEvent.EFFECT_EXECUTION_SYS_MINION_ENTER_THE_BATTLEFIELD:
                //    CardEntry enterTBFMinionCellItem = notification.Body as CardEntry;
                //    exeEffectCard = enterTBFMinionCellItem;
                //    提取待执行的卡牌效果
                //    List<EffectInfo> oneMinionEffectCardEffects = new List<EffectInfo>();
                //    遍历卡牌的效果
                //    foreach (string effectCode in exeEffectCard.effectCodeList)
                //    {
                //        EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                //        bool canExe = false;
                //        foreach (string impactTimeTrigger in oneEffectInfo.impactTimeTriggers) {
                //            if (impactTimeTrigger == "MyselfEnterTheBattlefield") {
                //                canExe = true;
                //                break;
                //            }
                //        }
                //        if (canExe) {
                //            设置状态
                //            oneEffectInfo.effectInfoStage = EffectInfoStage.UnStart;
                //            设置所有者,手牌操作模式，所有者是当前玩家
                //            oneEffectInfo.player = exeEffectCard.controllerPlayerItem;
                //            设置所属卡牌
                //            oneEffectInfo.cardEntry = exeEffectCard;

                //            oneMinionEffectCardEffects.Add(oneEffectInfo);
                //        }
                        
                //    }
                //    存入效果，进行结算
                //    effectInfoProxy.IntoModeCardSettle(exeEffectCard, oneMinionEffectCardEffects);
                //    break;
            }
        }
        //执行效果
        public void ExecutionEffectContentList(EffectInfoProxy effectInfoProxy, GameContainerProxy gameContainerProxy)
        {
            //if (effectInfoProxy.effectSysItem.cardEntry.gameContainerType == "CardHand") {
            //    effectInfoProxy.effectSysItem.cardEntry.ttCardNeedHideInView(effectInfoProxy.effectSysItem.cardEntry);
            //}
            for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
            {
                EffectInfo effectInfo = effectInfoProxy.effectSysItem.effectInfos[n];
                UtilityLog.Log("开始执行效果【" + effectInfo.code + "】", LogUtType.Effect);
                effectInfo.effectInfoStage = EffectInfoStage.Executing;
                if (effectInfo.effectType == "Independent")
                {
                    ExecutionOneEffectContent(effectInfoProxy, effectInfo, gameContainerProxy);
                }
                else if (effectInfo.effectType == "BeforeAndAfter")
                {
                    bool allPreEffectExe = true;
                    //遍历每一个前置效果
                    foreach (EffectInfo preEffect in effectInfo.preEffectEntryList)
                    {
                        UtilityLog.Log("执行前置效果", LogUtType.Special);
                        if (preEffect.userChooseExecution)
                        {
                            UtilityLog.Log("执行一个前置效果", LogUtType.Special);
                            ExecutionOneEffectContent(effectInfoProxy, preEffect, gameContainerProxy);
                        }
                        else {
                            allPreEffectExe = false;
                        }
                    }
                    if (allPreEffectExe) {
                        UtilityLog.Log("执行后置效果", LogUtType.Special);
                        //遍历每一个后置效果
                        foreach (EffectInfo postEffect in effectInfo.postEffectEntryList)
                        {
                            UtilityLog.Log("执行一个后置效果", LogUtType.Special);
                            ExecutionOneEffectContent(effectInfoProxy, postEffect, gameContainerProxy);
                        }
                    }
                    effectInfo.effectInfoStage = EffectInfoStage.Finished;

                }
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
        //执行一个效果
        public void ExecutionOneEffectContent(EffectInfoProxy effectInfoProxy, EffectInfo effectInfo, GameContainerProxy gameContainerProxy) {
            EffectExecution(effectInfo, EffectExeType.Execute, gameContainerProxy);
            effectInfo.effectInfoStage = EffectInfoStage.Finished;
            //进行效果执行完之后的检查
            //SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_PUT_ONE_EFFECT);
        }

        //为效果选择主语目标
        public bool ExecutionEffectFindTarget(  EffectInfo effectInfo,
                                            EffectInfoProxy effectInfoProxy,
                                            PlayerGroupProxy playerGroupProxy, 
                                            QuestStageCircuitProxy questStageCircuitProxy,
                                            GameContainerProxy gameContainerProxy)
        {
            bool hasTargetOver = true;
            if (effectInfo.effectInfoStage == EffectInfoStage.UnStart || effectInfo.effectInfoStage == EffectInfoStage.ConfirmingTarget)
            {
                effectInfo.effectInfoStage = EffectInfoStage.ConfirmingTarget;
                foreach (TargetSet targetSet in effectInfo.operationalTarget.selectTargetList)
                {
                    bool findOver = FindTargetOrObejct(effectInfo, targetSet, effectInfoProxy, playerGroupProxy, questStageCircuitProxy, gameContainerProxy);
                    if (!findOver)
                    {
                        return false;
                    }
                }
                foreach (TargetSet targetSet in effectInfo.operationalTarget.selectTargetList)
                {
                    if (!targetSet.hasTarget)
                    {
                        hasTargetOver = false;
                    }
                }
                if (hasTargetOver)
                {
                    effectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                }
            }
          
           
            return hasTargetOver;

        }
       

        //为前置后置效果选择主语目标
        public bool ExecutionEffectFindTargetForBeforeAndAfter(EffectInfo effectInfo,
                                            PlayerGroupProxy playerGroupProxy,
                                            QuestStageCircuitProxy questStageCircuitProxy,
                                            EffectInfoProxy effectInfoProxy,
                                            GameContainerProxy gameContainerProxy
                                            )
        {
            effectInfo.effectInfoStage = EffectInfoStage.ConfirmingTarget;
            //遍历每一个前置效果
            foreach (EffectInfo preEffect in effectInfo.preEffectEntryList) {
                preEffect.player = effectInfo.player;
                preEffect.cardEntry = effectInfo.cardEntry;
                bool hasTarget = ExecutionEffectFindTarget(preEffect, effectInfoProxy, playerGroupProxy, questStageCircuitProxy, gameContainerProxy);
                if (!hasTarget) {
                    return hasTarget;
                }
            }
            //遍历每一个后置效果
            foreach (EffectInfo postEffect in effectInfo.postEffectEntryList)
            {
                postEffect.player = effectInfo.player;
                postEffect.cardEntry = effectInfo.cardEntry;
                bool hasTarget = ExecutionEffectFindTarget(postEffect, effectInfoProxy, playerGroupProxy, questStageCircuitProxy, gameContainerProxy);
                if (!hasTarget)
                {
                    return hasTarget;
                }
            }
            effectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;
            return true;
        }
        public void AskTheUserOperating(EffectInfo effectInfo,
                                            PlayerGroupProxy playerGroupProxy,
                                            QuestStageCircuitProxy questStageCircuitProxy,
                                            EffectInfoProxy effectInfoProxy,
                                            GameContainerProxy gameContainerProxy) {
            effectInfo.effectInfoStage = EffectInfoStage.AskTheUser;
            if (effectInfo.effectType == "Independent")
            {
               
            }
            else if (effectInfo.effectType == "BeforeAndAfter")
            {
                //遍历每一个前置效果
                foreach (EffectInfo preEffect in effectInfo.preEffectEntryList)
                {
                    if (preEffect.effectInfoStage == EffectInfoStage.AskTheUser
                            || preEffect.effectInfoStage == EffectInfoStage.ConfirmedTarget) {
                        preEffect.effectInfoStage = EffectInfoStage.AskTheUser;
                        preEffect.checkCanExecution = true;

                        bool checkOver = EffectExecution(preEffect, EffectExeType.Check, gameContainerProxy);
                        if (!checkOver)
                        {
                            preEffect.checkCanExecution = false;
                            UtilityLog.Log(preEffect.code + ":检查为不能释放", LogUtType.Special);
                        }
                        //判断是否是必发
                        if (preEffect.mustBeLaunched == "N")
                        {
                            //不是必发，需要用户判断
                            effectInfo.needChoosePreEffect = preEffect;
                            //发送已经确认目标的效果到前台进行展示
                            CardEntry oneCardEntry = effectInfo.cardEntry;
                            oneCardEntry.needShowEffectInfo = effectInfo;
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            SendNotification(UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS, oneCardEntry, UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_ONE_EFFECT_NEED_CHOOSE_EXE);
                            return;

                        }
                        else
                        {
                            preEffect.effectInfoStage = EffectInfoStage.AskTheUserOver;
                        }
                    }
                    else
                    {
                        UtilityLog.LogError("逻辑错误：不应出现【用户判断】阶段前不是【宾语目标寻找结束】阶段");
                    }
                    
                }
            }
           
            effectInfo.effectInfoStage = EffectInfoStage.AskTheUserOver;

        }

        public bool FindTargetOrObejct(EffectInfo effectInfo,TargetSet targetSet,
                                           EffectInfoProxy effectInfoProxy,
                                           PlayerGroupProxy playerGroupProxy,
                                           QuestStageCircuitProxy questStageCircuitProxy,
                                           GameContainerProxy gameContainerProxy)
        {
            bool designationTargetOver = false;
            effectInfo.needPlayerToChooseTargetSet = targetSet;
            //先判断是否已经指定了目标
            switch (targetSet.target)
            {
                case "Minion":
                    if (targetSet.targetMinionCellItems.Count == targetSet.targetClaimsNums)
                    {
                        designationTargetOver = true;
                    }
                    break;
            }
            if (designationTargetOver)
            {
                targetSet.hasTarget = true;
                return true;
            }
            //条件
            TargetClaim[] targetClaims = targetSet.targetClaims;

            //目标玩家
            PlayerItem targetPlayer = null;
            //类型
            switch (targetSet.target)
            {
                case "Card":
                    List<CardEntry> cardEntries = new List<CardEntry>();
                    string gameContainerType = targetSet.targetSource;
                    PlayerItem gameContainerControllerPlayerItem = null;
                    for (int n = 0; n < targetClaims.Length; n++)
                    {
                        //判断所有权
                        if (targetClaims[n].claim == "Owner")
                        {
                            //是自己选
                            if (targetClaims[n].content == "Myself")
                            {
                                gameContainerControllerPlayerItem = effectInfo.player;
                            }
                        }
                    }
                    GameContainerItem gameContainerItem = gameContainerProxy.GetGameContainerItemByPlayerItemAndGameContainerType(gameContainerControllerPlayerItem, gameContainerType);
                    for (int n = 0; n < targetClaims.Length; n++)
                    {
                        //判断所有权
                        if (targetClaims[n].claim == "locationIndex")
                        {
                            cardEntries.Add(gameContainerItem.cardEntryList[Convert.ToInt32(targetClaims[n].content)]);
                        }
                        else if (targetClaims[n].claim == "CardCode") {
                            CardEntry cardEntryByCode = gameContainerItem.GetOneCardByCardCode(targetClaims[n].content);
                            if (cardEntryByCode != null) {
                                cardEntries.Add(cardEntryByCode);
                            }
                        }
                    }
                    if (cardEntries.Count > 0)
                    {
                        if (targetSet.targetClaimsNums == 1)
                        {
                            targetSet.hasTarget = true;
                            targetSet.targetCardEntries.Add(cardEntries[0]);
                        }
                    }
                    break;
                case "Minion":
                    List<CardEntry> minionCellItems = new List<CardEntry>();
                    List<GameContainerItem> returnGameContainerItemList = gameContainerProxy.GetGameContainerItemGameContainerType("CardBattlefield");
                    foreach (GameContainerItem gameContainerItemMinion in returnGameContainerItemList)
                    {
                        foreach (CardEntry minionCellItem in gameContainerItemMinion.cardEntryList)
                        {
                            for (int n = 0; n < targetClaims.Length; n++)
                            {
                                //判断所有权
                                if (targetClaims[n].claim == "Owner")
                                {
                                    //是自己的
                                    if (targetClaims[n].content == "Myself")
                                    {
                                        targetClaims[n].result.Add(effectInfo.player.playerCode);
                                        if (minionCellItem.controllerPlayerItem.playerCode == effectInfo.player.playerCode)
                                        {
                                            minionCellItems.Add(minionCellItem);
                                        }
                                    }
                                    //不是自己的
                                    else if (targetClaims[n].content == "Enemy")
                                    {
                                        targetClaims[n].result.Add(effectInfo.player.playerCode);
                                        if (minionCellItem.controllerPlayerItem.playerCode != effectInfo.player.playerCode)
                                        {
                                            minionCellItems.Add(minionCellItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (minionCellItems.Count <= targetSet.targetClaimsNums)
                    {
                        //符合数量限制
                        targetSet.hasTarget = true;
                        targetSet.targetMinionCellItems = minionCellItems;
                    }
                    else
                    {
                        //超出目标上限，需要用户选择
                        if (effectInfo.operationalTarget.whoOperate == "MyselfPlayer")
                        {
                            effectInfo.chooseByPlayer = effectInfo.player;
                            //发送已经确认目标的效果到前台进行展示
                            CardEntry oneCardEntry = effectInfo.cardEntry;
                            oneCardEntry.needShowEffectInfo = effectInfo;
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            SendNotification(UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS, oneCardEntry, UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_ONE_EFFECT_NEED_CHOOSE_TARGET);
                            //发布用户需要选择信号
                            SendNotification(LogicalSysEvent.LOGICAL_SYS, effectInfo, LogicalSysEvent.LOGICAL_SYS_NEED_PLAYER_CHOOSE);
                        }
                        else {
                            UtilityLog.LogError("找不到需要选择的用户");
                        }
                        return false;
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
                            if (targetClaims[n].claim == "Owner")
                            {
                                //是自己选
                                if (targetClaims[n].content == "Myself")
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
                        SendNotification(LogicalSysEvent.LOGICAL_SYS, effectInfo, LogicalSysEvent.LOGICAL_SYS_NEED_PLAYER_CHOOSE);
                        return false;
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
                            if (targetClaims[n].claim == "Owner")
                            {
                                //是自己选
                                if (targetClaims[n].content == "Myself")
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
            return true;
        }

        public bool EffectExecution(EffectInfo exeEffect, EffectExeType effectExeType, GameContainerProxy gameContainerProxy)
        {
            if (exeEffect.operationalContent != null)
            {
                if (exeEffect.operationalContent.operationalTarget == "All")
                {
                    foreach (TargetSet targetSetDto in exeEffect.operationalTarget.selectTargetList)
                    {
                        if (targetSetDto.target == "Minion")
                        {
                            for (int n = 0; n < targetSetDto.targetMinionCellItems.Count; n++)
                            {
                                if (!exeEffect.isReverse)
                                {
                                    if (exeEffect.effectiveTime.ContinuousStage != "Moment" && exeEffect.effectiveTime.ContinuousStage != "Permanent")
                                    {
                                        targetSetDto.targetMinionCellItems[n].effectBuffInfoList.Add(exeEffect);
                                        targetSetDto.targetMinionCellItems[n].ttBuffChange();
                                    }
                                }
                                for (int m = 0; m < exeEffect.operationalContent.impactTargets.Length; m++)
                                {
                                    bool result = ChangeOrCheckMinion(exeEffect.operationalContent.impactTargets[m],
                                                 exeEffect.operationalContent.impactContents[m],
                                                 targetSetDto.targetMinionCellItems[n],
                                                 exeEffect.isReverse,
                                                 effectExeType);
                                }
                            }
                        }
                        else if (targetSetDto.target == "Player")
                        {
                            for (int n = 0; n < targetSetDto.targetPlayerItems.Count; n++)
                            {
                                UtilityLog.Log("目标玩家【" + targetSetDto.targetPlayerItems[n].playerCode + "】生效效果【" + exeEffect.description + "】", LogUtType.Effect);
                                for (int m = 0; m < exeEffect.operationalContent.impactTargets.Length; m++)
                                {
                                    bool result = ChangeOrCheckPlayer(
                                        exeEffect.operationalContent.impactTargets[m],
                                        exeEffect.operationalContent.impactContents[m],
                                        targetSetDto.targetPlayerItems[n],
                                        effectExeType);
                                    if (!result)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        else if (targetSetDto.target == "Card")
                        {
                            for (int n = 0; n < targetSetDto.targetCardEntries.Count; n++)
                            {
                                for (int m = 0; m < exeEffect.operationalContent.impactTargets.Length; m++)
                                {
                                    bool result = ChangeOrCheckCard(
                                        exeEffect.operationalContent.impactTargets[m],
                                        exeEffect.operationalContent.impactContents[m],
                                        targetSetDto.targetCardEntries[n],
                                        gameContainerProxy,
                                        effectExeType);
                                    if (!result)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;

        }


      
        bool ChangeOrCheckMinion(string impactTarget, string impactContent, CardEntry minionCellItem, bool isReverse, EffectExeType effectExeType)
        {
            bool canExecute = true;
            switch (impactTarget)
            {
                case "ATK":
                    minionCellItem.cardEntryVariableAttributeMap.ChangeValueByCodeAndTypeAndIsReverse("Atk", Convert.ToInt32(impactContent), isReverse);
                    minionCellItem.ttAttributeChange();
                    break;
                case "DEF":
                    minionCellItem.cardEntryVariableAttributeMap.ChangeValueByCodeAndTypeAndIsReverse("Def", Convert.ToInt32(impactContent), isReverse);
                    minionCellItem.ttAttributeChange();
                    break;
                case "Attack":
                    minionCellItem.ttLaunchAnAttack();
                    break;
                case "Move":
                    minionCellItem.ttLaunchAnMove();
                    break;
                case "Life":
                    if (impactContent == "Sacrifice") {
                        if (effectExeType == EffectExeType.Check)
                        {
                            canExecute = true;
                        }
                        else if (effectExeType == EffectExeType.Execute)
                        {
                            minionCellItem.ttMinionToSacrifice();
                        }
                    }
                    break;
            }
            return canExecute;
        }
        bool ChangeOrCheckCard(string impactTarget, string impactContent, CardEntry cardEntry, GameContainerProxy gameContainerProxy,EffectExeType effectExeType)
        {
            bool canExecute = true;
            if (impactTarget == "GameContainerType") {
                gameContainerProxy.MoveOneCardFromOldeContainerItemToNeweContainerItem(cardEntry,impactContent);
            }

            return canExecute;

        }
        bool ChangeOrCheckPlayer(string impactTarget, string impactContent, PlayerItem playerItem, EffectExeType effectExeType)
        {
            bool canExecute = true;
            switch (impactTarget)
            {
                case "Hand":
                    playerItem.DrawCard(Convert.ToInt32(impactContent));
                    break;
                //资源上限
                case "ManaUpperLimit":
                    playerItem.ChangeManaUpperLimit(Convert.ToInt32(impactContent));
                    break;
                case "ManaUsable":
                    if (effectExeType == EffectExeType.Execute)
                    {
                        //恢复至上限使用Max,是不能被转为数字的
                        if (impactContent == "Max")
                        {
                            playerItem.RestoreToTheUpperLimit();
                        }
                        else
                        {
                            playerItem.ChangeManaUsable(Convert.ToInt32(impactContent));
                        }
                    }
                    else if (effectExeType == EffectExeType.Check)
                    {
                        //恢复至上限使用Max,是不能被转为数字的
                        if (impactContent == "Max")
                        {

                        }
                        else
                        {
                            canExecute = playerItem.CheckCanChangeManaUsable(Convert.ToInt32(impactContent));
                        }

                    }
                    break;
                //科技相关
                case "TraitAddOne":
                    playerItem.AddTraitType(impactContent);
                    break;
                case "Score":
                    playerItem.ChangeSocre(Convert.ToInt32(impactContent));
                    break;
                case "CanUseResourceNum":
                    if (impactContent == "Max")
                    {
                        // 使用资源牌恢复到最大次数
                        playerItem.RestoreCanUseResourceNumMax();
                    }
                    else
                    {

                    }
                    break;

            }
            return canExecute;
        }
    }
}
