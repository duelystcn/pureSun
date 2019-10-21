using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Persistence;
using Assets.Scripts.OrderSystem.Model.Database.TestCase;
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
using Assets.Scripts.OrderSystem.View.UIView;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;

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
                HandViewMediator handGridMediator = new HandViewMediator(mainUI.HandControlView);
                Facade.RegisterMediator(handGridMediator);
            }
           



            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            HexGridProxy hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            QuestStageCircuitProxy questStageCircuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;

            switch (notification.Type) {
                case OrderSystemEvent.START_CIRCUIT_MAIN:
                    SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, null, StringUtil.GetNTByNotificationTypeAndUIViewName(UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW, UIViewConfig.getNameStrByUIViewName(UIViewName.StartMain)));
                    SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, null, StringUtil.GetNTByNotificationTypeAndUIViewName(UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW, UIViewConfig.getNameStrByUIViewName(UIViewName.CardMoveAnimation)));
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
                        SendNotification(
                            UIViewSystemEvent.UI_VIEW_CURRENT, 
                            playerItem, 
                            StringUtil.GetNTByNotificationTypeAndPlayerCodeAndUIViewName(
                                UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW, 
                                playerItem.playerCode, 
                                UIViewConfig.getNameStrByUIViewName(UIViewName.CardDeckList)
                                )
                            );
                    }
                    //开启选择阶段
                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, null, UIViewSystemEvent.UI_CHOOSE_STAGE_START);
                   
                    break;
                case OrderSystemEvent.START_CIRCUIT_TEST_CASE:
                    TestCaseProxy testCaseProxy = Facade.RetrieveProxy(TestCaseProxy.NAME) as TestCaseProxy;
                    List<TestCaseInfo> testCaseInfoList = testCaseProxy.testCaseInfoMap.Values.ToList();
                    SendNotification(
                          UIViewSystemEvent.UI_VIEW_CURRENT,
                          testCaseInfoList,
                          StringUtil.GetNTByNotificationTypeAndUIViewName(
                              UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                              UIViewConfig.getNameStrByUIViewName(UIViewName.TestCaseView)
                              )
                          );
                    break;
                //开始一个测试
                case OrderSystemEvent.START_CIRCUIT_TEST_CASE_START_ONE:
                    TestCaseInfo chooseOneTestCase = notification.Body as TestCaseInfo;
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
                        PI_Player pI_Player = new PI_Player();
                        if (playerItem.playerCode == "TEST1")
                        {
                            pI_Player = chooseOneTestCase.myselfPlayer;
                            playerItem.CreateCanCallHex(questStageCircuitProxy.hexModelInfo, hexGridProxy.HexGrid.cells, true);
                        }
                        else {
                            pI_Player = chooseOneTestCase.enemyPlayer;
                            playerItem.CreateCanCallHex(questStageCircuitProxy.hexModelInfo, hexGridProxy.HexGrid.cells, false);
                        }
                        //创建牌库
                        playerItem.cardDeck = new CardDeck();
                        foreach (string cardName in pI_Player.deckCard) {
                            CardEntry cardEntry = new CardEntry();
                            cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(cardName));
                            playerItem.cardDeck.cardEntryList.Add(cardEntry);
                        }
                        CardEntry shipCard = new CardEntry();
                        shipCard.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(pI_Player.shipCardCode));
                        playerItem.shipCard = shipCard;
                        //手牌
                        foreach (string cardName in pI_Player.handCard)
                        {
                            CardEntry cardEntry = new CardEntry();
                            cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(cardName));
                            playerItem.AddCardToHandAndNoTT(cardEntry);
                            
                        }
                    }
                    SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.TestCaseView), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                    SendNotification(UIViewSystemEvent.UI_QUEST_STAGE, null, UIViewSystemEvent.UI_QUEST_STAGE_START_SPECIAL);
                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, null, OperateSystemEvent.OPERATE_TRAIL_DRAW_CREATE);
                    break;
                //开始测试地图
                case OrderSystemEvent.START_CIRCUIT_TEST_MAP:
                   
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
