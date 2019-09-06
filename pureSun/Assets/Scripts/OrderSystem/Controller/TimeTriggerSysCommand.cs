using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
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
            string playerCodeNotification = StringUtil.GetPlayerCodeForNP(notification.Type);
            notification.Type = StringUtil.GetNotificationTypeForNP(notification.Type);

            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            switch (notification.Type)
            {
                case TimeTriggerEvent.TIME_TRIGGER_SYS_HAND_CAN_USE_JUDGE:
                    string playerCode = notification.Body as string;
                    PlayerItem playerItem = playerGroupProxy.playerGroup.playerItems[playerCode];
                    foreach (HandCellItem handCellItem in playerItem.handGridItem.handCells) {
                        handCellItem.canUse = playerItem.checkOneCardCanUse(handCellItem.cardEntry);
                    }
                    SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem.handCells, HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE);
                    break;
                case TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD:
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD)) {
                        List<EffectInfo> effectInfos = circuitProxy.circuitItem.activeEffectInfoMap[TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD];
                        foreach(EffectInfo effectInfo in effectInfos) {
                            foreach (ImpactTimeTrigger impactTimeTrigger in effectInfo.impactTimeTriggerList) {
                                if (impactTimeTrigger.impactTimeTriggertMonitor == TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD) {
                                    //触发了抽牌效果
                                    for (int n = 0; n < impactTimeTrigger.impactTimeTriggertClaims.Length; n++) {
                                        //判断触发点的所有权
                                        if (impactTimeTrigger.impactTimeTriggertClaims[n] == "Owner") {
                                            //触发点是自己
                                            if (impactTimeTrigger.impactTimeTriggertClaimsContents[n] == "Myself") {
                                                if (playerCodeNotification == effectInfo.player.playerCode) {
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
                    break;
            }
        }
    }
}
