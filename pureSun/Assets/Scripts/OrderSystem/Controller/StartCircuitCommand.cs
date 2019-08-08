using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class StartCircuitCommand: SimpleCommand
    {
        //开始流程
        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            switch (notification.Type) {
                case OrderSystemEvent.START_CIRCUIT_MAIN:
                    SendNotification(UIViewSystemEvent.UI_START_MAIN, null, UIViewSystemEvent.UI_START_MAIN_OPEN);
                    break;
                case OrderSystemEvent.START_CIRCUIT_START:
                    //CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
                    //玩家信息初始化
                    playerGroupProxy.AddPlayer("TEST1",PlayerType.HumanPlayer);
                    playerGroupProxy.AddPlayer("TEST2", PlayerType.AIPlayer);

                    //设定UI段显示为玩家TEST1
                    SendNotification(OrderSystemEvent.CLINET_SYS, "TEST1", OrderSystemEvent.CLINET_SYS_OWNER_CHANGE);

                    ChooseStageCircuitProxy chooseStageCircuitProxy = Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
                    chooseStageCircuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);
                    //开启卡组列渲染
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        SendNotification(UIViewSystemEvent.UI_CARD_DECK_LIST, playerItem, StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_CARD_DECK_LIST_OPEN, playerItem.playerCode));
                    }
                    //开启选择阶段
                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, null, UIViewSystemEvent.UI_CHOOSE_STAGE_START);
                   
                    break;
                case OrderSystemEvent.START_CIRCUIT_TEST_MAP:
                    CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
                    //玩家信息初始化
                    playerGroupProxy.AddPlayer("TEST1", PlayerType.HumanPlayer);
                    playerGroupProxy.AddPlayer("TEST2", PlayerType.AIPlayer);
                    //设定UI段显示为玩家TEST1
                    SendNotification(OrderSystemEvent.CLINET_SYS, "TEST1", OrderSystemEvent.CLINET_SYS_OWNER_CHANGE);
                    //设置虚拟坐标
                    playerGroupProxy.playerGroup.playerItems["TEST1"].hexCoordinates = new HexCoordinates(0, -1);
                    playerGroupProxy.playerGroup.playerItems["TEST2"].hexCoordinates = new HexCoordinates(0, 4);
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        //创建牌库
                        playerItem.cardDeck = new CardDeck();
                        List<CardEntry> cardEntryList = new List<CardEntry>();
                        for (int i = 0; i < 20; i++)
                        {
                            CardEntry cardEntry = new CardEntry();
                            if (i % 3 == 0)
                            {
                                //生物
                                cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("ImperialRecruit"));
                            }
                            else if (i % 3 == 1)
                            {
                                //事件
                                cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("FortifiedAgent"));
                            }
                            else
                            {
                                //资源
                                cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("TaxCar"));
                            }
                            cardEntryList.Add(cardEntry);
                        }
                        playerItem.cardDeck.cardEntryList = cardEntryList;
                    }


                    SendNotification(UIViewSystemEvent.UI_QUEST_STAGE, null, UIViewSystemEvent.UI_QUEST_STAGE_START);
                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, null, OperateSystemEvent.OPERATE_TRAIL_DRAW_CREATE);
                    break;
            }
            
        }


    }
}
