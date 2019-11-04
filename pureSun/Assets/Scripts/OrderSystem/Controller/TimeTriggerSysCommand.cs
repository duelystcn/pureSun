using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    public class TimeTriggerSysCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            string playerCodeNotification = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "PlayerCode");
            string notificationType = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "NotificationType");

            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;

            MinionGridProxy minionGridProxy =
              Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;

            EffectInfoProxy effectInfoProxy =
              Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            //一组效果执行完毕
            if (notification.Type == TimeTriggerEvent.TIME_TRIGGER_EXE_NEXT_DELAY_NOTIFICATION) {
                if (effectInfoProxy.effectSysItem.delayNotifications.Count > 0)
                {
                    notification = effectInfoProxy.effectSysItem.delayNotifications.Dequeue();
                    playerCodeNotification = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "PlayerCode");
                    notificationType = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "NotificationType");
                }
                else {
                    return;
                }
            }

            //判断是否在一组效果的执行中
            if (effectInfoProxy.effectSysItem.effectSysItemStage == EffectSysItemStage.Executing) {
                effectInfoProxy.effectSysItem.delayNotifications.Enqueue(notification);
                return;
            }

            switch (notificationType)
            {
              
                //抽了一张牌
                case TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD:
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD)) {
                        SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD], playerCodeNotification, notificationType);
                    }
                    break;
                //一个阶段的执行
                case TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_STAGE_EXECUTION:
                    string oneTurnStageStart = notification.Body as string + "Execution";
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(oneTurnStageStart))
                    {
                        SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[oneTurnStageStart], playerCodeNotification, oneTurnStageStart);
                    }
                    break;
                //一个回合的结束，检查是否有需要清除的buff
                case TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_TURN_END:
                    //检测生物
                    foreach (MinionCellItem minionCellItem in minionGridProxy.minionGridItem.minionCells.Values)
                    {
                        minionCellItem.CheckNeedChangeEffectBuffInfo("EndOfTurn");
                    }

                    //检测玩家

                    break;
                //使用了一张手牌
                case TimeTriggerEvent.TIME_TRIGGER_SYS_USE_HAND_CARD:
                    HandCellItem handCellItemUse = notification.Body as HandCellItem;
                    switch (handCellItemUse.cardEntry.WhichCard) {
                        case CardEntry.CardType.ResourceCard:
                            playerGroupProxy.playerGroup.playerItems[handCellItemUse.cardEntry.player.playerCode].canUseResourceNum--;
                            break;

                    }
                    break;
            }
            //判断还有没有需要继续触发的时点信息
            if (effectInfoProxy.effectSysItem.delayNotifications.Count > 0) {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, null, TimeTriggerEvent.TIME_TRIGGER_EXE_NEXT_DELAY_NOTIFICATION);
            }
        }
        //触发后判断效果是否可以执行
        public void SelectEffectAfterTrigger(List<EffectInfo> effectInfos, string playerCodeNotification, string notificationType) {
            foreach (EffectInfo effectInfo in effectInfos)
            {
               
                //如果是规则效果，那么需要将效果所有者设置成当前玩家
                if (effectInfo.impactType == "GameModelRule") {
                    effectInfo.player.playerCode = playerCodeNotification;
                }
                UtilityLog.Log("【" + effectInfo.player.playerCode + "】拥有的效果【" + effectInfo.description + "】,触发者【" + playerCodeNotification + "】检测是否可以触发成功", LogUtType.Effect);
                foreach (ImpactTimeTrigger impactTimeTrigger in effectInfo.impactTimeTriggerList)
                {
                    if (impactTimeTrigger.impactTimeTriggertMonitor == notificationType)
                    {
                        //触发了效果
                        for (int n = 0; n < impactTimeTrigger.impactTimeTriggertClaims.Length; n++)
                        {
                            //判断触发点的所有权
                            if (impactTimeTrigger.impactTimeTriggertClaims[n] == "Owner")
                            {
                                //触发点是自己
                                if (impactTimeTrigger.impactTimeTriggertClaimsContents[n] == "Myself")
                                {
                                    if (playerCodeNotification == effectInfo.player.playerCode)
                                    {
                                        //触发成功，要先清除掉效果的目标列
                                        effectInfo.CleanEffectTargetSetList();
                                        //设置状态
                                        effectInfo.effectInfoStage = EffectInfoStage.UnStart;
                                        effectInfo.cardEntry.triggeredEffectInfo = effectInfo;
                                        UtilityLog.Log("效果【" + effectInfo.description + "】被【"+ playerCodeNotification + "】触发", LogUtType.Effect);
                                        SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, effectInfo.cardEntry, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_TRIGGERED_CARD);
                                    }
                                }
                            }
                        }
                    } 
                }
            }
        }
    }
}
