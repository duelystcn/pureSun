using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
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
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            GameContainerProxy gameContainerProxy =
                        Facade.RetrieveProxy(GameContainerProxy.NAME) as GameContainerProxy;

            string playerCodeNotification = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "PlayerCode");
            string notificationType = StringUtil.GetValueForNotificationTypeByKey(notification.Type, "NotificationType");
            






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


            PlayerItem playerItemNotification = null;
            if (playerCodeNotification != null)
            {
                playerItemNotification = playerGroupProxy.getPlayerByPlayerCode(playerCodeNotification);
            }
           

            switch (notificationType)
            {
                //玩家需要抽一张牌
                case TimeTriggerEvent.TIME_TRIGGER_SYS_NEED_DRAW_A_CARD:
                    SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[TimeTriggerEvent.TIME_TRIGGER_SYS_NEED_DRAW_A_CARD],null, playerItemNotification, notificationType);
                    break;
                //抽了一张牌
                case TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD:
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD)) {
                        SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD], null,playerItemNotification, notificationType);
                    }
                    break;
                //一个阶段的执行
                case TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_STAGE_EXECUTION:
                    string oneTurnStageStart = notification.Body as string + "Execution";
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(oneTurnStageStart))
                    {
                        SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[oneTurnStageStart],null, playerItemNotification, oneTurnStageStart);
                    }
                    break;
                //一个回合的结束，检查是否有需要清除的buff
                case TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_TURN_END:
                    //检测生物
                    List<CardEntry> minionCellItems = gameContainerProxy.GetCardEntryListByGameContainerType("CardBattlefield");
                    foreach (CardEntry minionCellItem in minionCellItems)
                    {
                        minionCellItem.CheckNeedChangeEffectBuffInfo("EndOfTurn");
                    }

                    //检测玩家

                    break;
                //使用了一张手牌
                case TimeTriggerEvent.TIME_TRIGGER_SYS_USE_HAND_CARD:
                    CardEntry handCellItemUse = notification.Body as CardEntry;
                    switch (handCellItemUse.WhichCard) {
                        case CardEntry.CardType.ResourceCard:
                            handCellItemUse.controllerPlayerItem.canUseResourceNum--;
                            break;

                    }
                    break;
                //卡牌到了新位置
                case TimeTriggerEvent.TIME_TRIGGER_SYS_CARD_CHANGE_GAME_CONTAINER_TYPE:
                    CardEntry enterTheNewTypeCard = notification.Body as CardEntry;
                    string cardEnterTheNewType = "CardEnterThe" + enterTheNewTypeCard.gameContainerType;
                    if (circuitProxy.circuitItem.activeEffectInfoMap.ContainsKey(cardEnterTheNewType))
                    {
                        SelectEffectAfterTrigger(circuitProxy.circuitItem.activeEffectInfoMap[cardEnterTheNewType], enterTheNewTypeCard, playerItemNotification, cardEnterTheNewType);
                    }
                    break;
            }
            //判断还有没有需要继续触发的时点信息
            if (effectInfoProxy.effectSysItem.delayNotifications.Count > 0) {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, null, TimeTriggerEvent.TIME_TRIGGER_EXE_NEXT_DELAY_NOTIFICATION);
            }
        }
        //触发后判断效果是否可以执行
        public void SelectEffectAfterTrigger(List<EffectInfo> effectInfos,CardEntry ttsCardEntry, PlayerItem playerItemNotification, string notificationType) {
            foreach (EffectInfo effectInfo in effectInfos)
            {
               
                //如果是规则效果，那么需要将效果所有者设置成当前玩家
                if (effectInfo.impactType == "GameModelRule") {
                    effectInfo.player = playerItemNotification;
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
                                    if (playerItemNotification == effectInfo.player)
                                    {
                                        //触发成功，要先清除掉效果的目标列
                                        effectInfo.CleanEffectTargetSetList();
                                        //设置状态
                                        effectInfo.effectInfoStage = EffectInfoStage.UnStart;
                                        effectInfo.cardEntry.triggeredEffectInfo = effectInfo;
                                        UtilityLog.Log("效果【" + effectInfo.description + "】被【"+ playerItemNotification.playerCode + "】触发", LogUtType.Effect);
                                        SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, effectInfo.cardEntry, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_TRIGGERED_CARD);
                                    }
                                }
                            }
                            if (impactTimeTrigger.impactTimeTriggertClaims[n] == "UUId") {
                                if (impactTimeTrigger.impactTimeTriggertClaimsContents[n] == "ThisCard") 
                                {
                                    if (ttsCardEntry.uuid == effectInfo.cardEntry.uuid) {
                                        //触发成功，要先清除掉效果的目标列
                                        effectInfo.CleanEffectTargetSetList();
                                        //设置状态
                                        effectInfo.effectInfoStage = EffectInfoStage.UnStart;
                                        effectInfo.cardEntry.triggeredEffectInfo = effectInfo;
                                        UtilityLog.Log("效果【" + effectInfo.description + "】被【" + ttsCardEntry.code + "】触发", LogUtType.Effect);
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
