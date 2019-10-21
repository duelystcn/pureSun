﻿

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    public class QuestStageCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            EffectInfoProxy effectInfoProxy = Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;

            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_QUEST_STAGE_START_SPECIAL:
                    //通知渲染战场
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_SYS_SHOW_START);
                    //渲染费用栏
                    SendNotification(
                                        UIViewSystemEvent.UI_VIEW_CURRENT,
                                        null,
                                        StringUtil.GetNTByNotificationTypeAndUIViewName(
                                            UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                            UIViewConfig.getNameStrByUIViewName(UIViewName.ManaInfoView)
                                            )
                                        );
                    //渲染科技栏
                    SendNotification(
                                      UIViewSystemEvent.UI_VIEW_CURRENT,
                                      null,
                                      StringUtil.GetNTByNotificationTypeAndUIViewName(
                                          UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                          UIViewConfig.getNameStrByUIViewName(UIViewName.TraitCombinationView)
                                          )
                                      );
                    //渲染船只栏
                    SendNotification(
                                      UIViewSystemEvent.UI_VIEW_CURRENT,
                                      null,
                                      StringUtil.GetNTByNotificationTypeAndUIViewName(
                                          UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                          UIViewConfig.getNameStrByUIViewName(UIViewName.ShipComponentView)
                                          )
                                      );

                    circuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);

                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        //先开启手牌栏
                        SendNotification(HandSystemEvent.HAND_VIEW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_VIEW_SYS_INIT_PLAYER_CODE, playerItem.playerCode));
                        //手牌渲染
                        SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_AFFLUX, playerItem.playerCode));
                        //设置起始费用上限
                        playerItem.manaItem.manaUpperLimit = 1;
                        //设置当前费用为0
                        playerItem.manaItem.manaUsable = 0;
                        //获取船只的效果，如果是持续效果则添加到全局监听中
                        foreach (string effectName in playerItem.shipCard.effectName)
                        {
                            EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectName);
                            if (oneEffectInfo.impactType == "Continue")
                            {
                                oneEffectInfo.player = playerItem;
                                oneEffectInfo.cardEntry = playerItem.shipCard;
                                circuitProxy.circuitItem.putOneEffectInfoInActiveMap(oneEffectInfo, effectInfoProxy.effectSysItem.impactTimeTriggerMap);
                            }

                        }
                        ManaItem manaItem = new ManaItem();
                        manaItem.manaUpperLimit = playerItem.manaItem.manaUpperLimit;
                        manaItem.manaUsable = playerItem.manaItem.manaUsable;
                        SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, manaItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_INIT, playerItem.playerCode));

                        SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, playerItem.traitCombination.traitTypes, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_INIT, playerItem.playerCode));
                    }
                    SendNotification(
                             UIViewSystemEvent.UI_VIEW_CURRENT,
                             null,
                             StringUtil.GetNTByNotificationTypeAndUIViewName(
                                 UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                 UIViewConfig.getNameStrByUIViewName(UIViewName.NextTurnButton)
                                 )
                             );
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);

                    break;
                case UIViewSystemEvent.UI_QUEST_STAGE_START:
                    //通知渲染战场
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_SYS_SHOW_START);
                    //渲染费用栏
                    SendNotification(
                                        UIViewSystemEvent.UI_VIEW_CURRENT,
                                        null,
                                        StringUtil.GetNTByNotificationTypeAndUIViewName(
                                            UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                            UIViewConfig.getNameStrByUIViewName(UIViewName.ManaInfoView)
                                            )
                                        );
                    //渲染科技栏
                    SendNotification(
                                       UIViewSystemEvent.UI_VIEW_CURRENT,
                                       null,
                                       StringUtil.GetNTByNotificationTypeAndUIViewName(
                                           UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                           UIViewConfig.getNameStrByUIViewName(UIViewName.TraitCombinationView)
                                           )
                                       );
                    //渲染船只栏
                    SendNotification(
                                       UIViewSystemEvent.UI_VIEW_CURRENT,
                                       null,
                                       StringUtil.GetNTByNotificationTypeAndUIViewName(
                                           UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                           UIViewConfig.getNameStrByUIViewName(UIViewName.ShipComponentView)
                                           )
                                       );

                    circuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);

                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        //先开启手牌栏
                        SendNotification(HandSystemEvent.HAND_VIEW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_VIEW_SYS_INIT_PLAYER_CODE, playerItem.playerCode));

                        //分发手牌
                        playerItem.DrawCard(5);
                        //设置起始费用上限
                        playerItem.manaItem.manaUpperLimit = 1;
                        //设置当前费用为0
                        playerItem.manaItem.manaUsable = 0;
                        //获取船只的效果，如果是持续效果则添加到全局监听中
                        foreach (string effectName in playerItem.shipCard.effectName)
                        {
                            EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectName);
                            if (oneEffectInfo.impactType == "Continue") {
                                oneEffectInfo.player = playerItem;
                                oneEffectInfo.cardEntry = playerItem.shipCard;
                                circuitProxy.circuitItem.putOneEffectInfoInActiveMap(oneEffectInfo, effectInfoProxy.effectSysItem.impactTimeTriggerMap);
                            }

                        }
                        ManaItem manaItem = new ManaItem();
                        manaItem.manaUpperLimit = playerItem.manaItem.manaUpperLimit;
                        manaItem.manaUsable = playerItem.manaItem.manaUsable;
                        SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, manaItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_INIT,playerItem.playerCode));

                        SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, playerItem.traitCombination.traitTypes, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_INIT, playerItem.playerCode));
                        //手牌渲染
                        //SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem, StringUtil.NotificationTypeAddPlayerCode(HandSystemEvent.HAND_CHANGE_AFFLUX, playerItem.playerCode));
                    }
                    SendNotification(
                            UIViewSystemEvent.UI_VIEW_CURRENT,
                            null,
                            StringUtil.GetNTByNotificationTypeAndUIViewName(
                                UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                UIViewConfig.getNameStrByUIViewName(UIViewName.NextTurnButton)
                                )
                            );
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);
                    break;
            }
        }
    }
}
