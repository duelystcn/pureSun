

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Controller
{
    //我希望把所有需要判断操作的都放到这里，来判断玩家操作和AI操作
    public class LogicalProcessorCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            ChooseStageCircuitProxy chooseStageCircuitProxy = Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
            EffectInfoProxy effectInfoProxy =
               Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            PlayerItem playerItem = null;
            switch (notification.Type)
            {
                //选择船只逻辑处理
                case LogicalSysEvent.LOGICAL_SYS_CHOOSE_SHIP_CARD:
                    string playerCode = notification.Body as string;
                    playerItem = playerGroupProxy.getPlayerByPlayerCode(playerCode);
                    //如果是玩家
                    if (playerItem.playerType == PlayerType.HumanPlayer)
                    {
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, 
                                         chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode], 
                                         StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY, playerCode));

                    }
                    //AI玩家
                    else if (playerItem.playerType == PlayerType.AIPlayer)
                    {
                        //AI进行船只渲染
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode], StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY, playerCode));
                        //AI选择船只
                        CardEntry shipCardEntry = chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode][0];
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, shipCardEntry, UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD);
                    }
                    //网络用户
                    else if (playerItem.playerType == PlayerType.NetPlayer)
                    {
                    }
                    break;
                case LogicalSysEvent.LOGICAL_SYS_CHOOSE_EFFECT:
                    EffectInfo effectInfo = notification.Body as EffectInfo;
                    playerItem = effectInfo.chooseByPlayer;
                    //如果是玩家
                    if (playerItem.playerType == PlayerType.HumanPlayer)
                    {
                        //整理好效果内容，弹出选择框让玩家选择
                        //把这些效果实例化成卡片
                        List<CardEntry> cardEntries = new List<CardEntry>();
                        foreach (string effectName in effectInfo.chooseEffectList) {
                            EffectInfo oneEffectInfo = effectInfoProxy.effectSysItem.effectInfoMap[effectName];
                            CardEntry oneCardEntry = new CardEntry();
                            oneCardEntry.InitializeByCardInfo(effectInfo.cardEntry.cardInfo);
                            oneCardEntry.effectName = new string[1]{effectName};
                            oneCardEntry.description = oneEffectInfo.description;
                            cardEntries.Add(oneCardEntry);
                        }
                        SendNotification(UIViewSystemEvent.UI_USER_OPERAT, cardEntries,
                                          StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_USER_OPERAT_CHOOSE_EFFECT, playerItem.playerCode));


                    }
                    //AI玩家
                    else if (playerItem.playerType == PlayerType.AIPlayer)
                    {

                      
                    }
                    //网络用户
                    else if (playerItem.playerType == PlayerType.NetPlayer)
                    {
                    }
                    break;
            }
        }
       
    }
   
}
