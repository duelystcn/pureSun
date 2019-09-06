using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.CircuitView.QuestStageCircuit;
using Assets.Scripts.OrderSystem.View.HandView;
using Assets.Scripts.OrderSystem.View.HexView;
using Assets.Scripts.OrderSystem.View.MinionView;
using Assets.Scripts.OrderSystem.View.OperateSystem;
using Assets.Scripts.OrderSystem.View.SpecialOperateView.ChooseView;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class StartCircuitCommand: SimpleCommand
    {
        //开始流程
        public override void Execute(INotification notification)
        {
            if (notification.Type == OrderSystemEvent.START_CIRCUIT_MAIN) {
                MainUI mainUI = notification.Body as MainUI;
                if (null == mainUI)
                    throw new Exception("程序启动失败..");


                //地图模式
                string arrayMode = HexMetrics.MODE_HORIZ;
                //地图大小
                int height = 4;
                int width = 6;

                HexModelInfo modelInfo = new HexModelInfo(width, height, arrayMode, HexModelType.Source);

                //地图代理(需要放在操作层之前)
                HexGridProxy hexGridProxyCreate = new HexGridProxy(modelInfo);
                Facade.RegisterProxy(hexGridProxyCreate);
                HexGridMediator hexGridMediator = new HexGridMediator(mainUI.HexGridView);
                Facade.RegisterMediator(hexGridMediator);

                //生物层代理
                MinionGridProxy minionGridProxy = new MinionGridProxy();
                Facade.RegisterProxy(minionGridProxy);
                MinionGridMediator minionGridMediator = new MinionGridMediator(mainUI.minionGridView);
                Facade.RegisterMediator(minionGridMediator);


                //进程代理
                QuestStageCircuitProxy createCircuitProxy = new QuestStageCircuitProxy(modelInfo);
                Facade.RegisterProxy(createCircuitProxy);
                QuestStageCircuitMediator circuitMediator = new QuestStageCircuitMediator(mainUI.circuitButton);
                Facade.RegisterMediator(circuitMediator);

                //操作系统代理
                OperateSystemProxy operateSystemProxy = new OperateSystemProxy(modelInfo);
                Facade.RegisterProxy(operateSystemProxy);
                OperateSystemMediator operateSystemMediator = new OperateSystemMediator(mainUI.operateSystemView);
                Facade.RegisterMediator(operateSystemMediator);

                //选择页面代理
                ChooseGridMediator chooseGridMediator = new ChooseGridMediator(mainUI.chooseGridView);
                Facade.RegisterMediator(chooseGridMediator);

                //手牌区代理（需要放在操作系统后）
                HandGridMediator handGridMediator = new HandGridMediator(mainUI.HandGridView);
                Facade.RegisterMediator(handGridMediator);
            }
           



            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            HexGridProxy hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            QuestStageCircuitProxy questStageCircuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;

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
                        //渲染可召唤区域
                        if (playerItem.playerCode == "TEST1")
                        {
                            playerItem.CreateCanCallHex(questStageCircuitProxy.hexModelInfo, hexGridProxy.HexGrid.cells, true);
                        }
                        else {
                            playerItem.CreateCanCallHex(questStageCircuitProxy.hexModelInfo, hexGridProxy.HexGrid.cells, false);
                        }

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
                        CardEntry shipCard = new CardEntry();
                        shipCard.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("FindWay"));
                        playerItem.shipCard = shipCard;

                    }




                    SendNotification(UIViewSystemEvent.UI_QUEST_STAGE, null, UIViewSystemEvent.UI_QUEST_STAGE_START);
                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, null, OperateSystemEvent.OPERATE_TRAIL_DRAW_CREATE);
                    break;
            }
            
        }


    }
}
