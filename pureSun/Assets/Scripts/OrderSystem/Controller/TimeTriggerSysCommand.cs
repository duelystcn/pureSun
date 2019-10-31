using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
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
            notification.Type = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "NotificationType");

            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            switch (notification.Type)
            {
                //判断手牌是否可用
                case TimeTriggerEvent.TIME_TRIGGER_SYS_HAND_CAN_USE_JUDGE:
                    string playerCode = notification.Body as string;
                    PlayerItem playerItem = playerGroupProxy.playerGroup.playerItems[playerCode];
                    playerItem.ChangeHandCardCanUse();
                    SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem.handCells, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE, playerCode));
                    break;
                //抽了一张牌
                case TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD:
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD)) {
                        SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD], playerCodeNotification, notification.Type);
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
        }
        //触发后判断效果是否可以执行
        public void SelectEffectAfterTrigger(List<EffectInfo> effectInfos, string playerCodeNotification, string notificationType) {
            foreach (EffectInfo effectInfo in effectInfos)
            {
                //如果是规则效果，那么需要将效果所有者设置成当前玩家
                if (effectInfo.impactType == "GameModelRule") {
                    effectInfo.player.playerCode = playerCodeNotification;
                }
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
