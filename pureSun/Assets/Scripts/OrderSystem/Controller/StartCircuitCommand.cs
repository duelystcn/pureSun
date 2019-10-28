using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
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

               

                

                //地图代理(需要放在操作层之前)
                HexGridProxy hexGridProxyCreate = new HexGridProxy();
                Facade.RegisterProxy(hexGridProxyCreate);
                HexGridMediator hexGridMediator = new HexGridMediator(mainUI.HexGridView);
                Facade.RegisterMediator(hexGridMediator);

                //生物层代理
                MinionGridProxy minionGridProxy = new MinionGridProxy();
                Facade.RegisterProxy(minionGridProxy);
                MinionGridMediator minionGridMediator = new MinionGridMediator(mainUI.minionGridView);
                Facade.RegisterMediator(minionGridMediator);


                //进程代理
                QuestStageCircuitProxy createCircuitProxy = new QuestStageCircuitProxy();
                Facade.RegisterProxy(createCircuitProxy);
                QuestStageCircuitMediator circuitMediator = new QuestStageCircuitMediator(mainUI.circuitButton);
                Facade.RegisterMediator(circuitMediator);

                //操作系统代理
                OperateSystemProxy operateSystemProxy = new OperateSystemProxy();
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
           
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            GameModelProxy gameModelProxy = Facade.RetrieveProxy(GameModelProxy.NAME) as GameModelProxy;

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
                    //玩家信息初始化
                    playerGroupProxy.AddPlayer("TEST1", PlayerType.HumanPlayer);
                    playerGroupProxy.AddPlayer("TEST2", PlayerType.AIPlayer);
                    //设定UI段显示为玩家TEST1
                    SendNotification(OrderSystemEvent.CLINET_SYS, "TEST1", OrderSystemEvent.CLINET_SYS_OWNER_CHANGE);
                    TestCaseInfo chooseOneTestCase = notification.Body as TestCaseInfo;
                   

                    SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.TestCaseView), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                    SendNotification(UIViewSystemEvent.UI_QUEST_STAGE, notification.Body, UIViewSystemEvent.UI_QUEST_STAGE_START_SPECIAL);
                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, null, OperateSystemEvent.OPERATE_TRAIL_DRAW_CREATE);
                    break;
                //开始测试地图
                case OrderSystemEvent.START_CIRCUIT_TEST_MAP:
                   
                    //获取当前模式有多少个玩家
                    //目前只考虑有两个玩家的情况
                    int num = gameModelProxy.gameModelNow.playerNum;
                    

                    //玩家信息初始化
                    playerGroupProxy.AddPlayer("TEST1", PlayerType.HumanPlayer);
                    playerGroupProxy.AddPlayer("TEST2", PlayerType.AIPlayer);
                    //设定UI段显示为玩家TEST1
                    SendNotification(OrderSystemEvent.CLINET_SYS, "TEST1", OrderSystemEvent.CLINET_SYS_OWNER_CHANGE);


                   
                    SendNotification(UIViewSystemEvent.UI_QUEST_STAGE, null, UIViewSystemEvent.UI_QUEST_STAGE_START);
                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, null, OperateSystemEvent.OPERATE_TRAIL_DRAW_CREATE);
                    break;
            }
            
        }
       

    }
}
