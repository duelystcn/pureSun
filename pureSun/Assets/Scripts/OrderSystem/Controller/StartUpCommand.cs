
/*=========================================
* Author: Administrator
* DateTime:2017/6/20 18:29:33
* Description:$safeprojectname$
==========================================*/

using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.TestCase;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.MinionView;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
/**
* 程序启动，初始化 
*/
namespace OrderSystem
{
    internal class StartUpCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
         

            MainUI mainUI = notification.Body as MainUI;
            if (null == mainUI)
                throw new Exception("程序启动失败..");

            //数据信息代理
            CardDbProxy cardDbProxy = new CardDbProxy();
            Facade.RegisterProxy(cardDbProxy);
            EffectInfoProxy effectInfoProxy = new EffectInfoProxy();
            Facade.RegisterProxy(effectInfoProxy);
            TestCaseProxy testCaseProxy = new TestCaseProxy();
            Facade.RegisterProxy(testCaseProxy);


            //UI层代理
            UIControllerListMediator uIControllerListMediator = new UIControllerListMediator(mainUI.UIControllerListView);
            Facade.RegisterMediator(uIControllerListMediator);


           

           

         
            //玩家组代理
            PlayerGroupProxy playerGroupProxy = new PlayerGroupProxy();
            Facade.RegisterProxy(playerGroupProxy);


            //选择阶段进程代理
            ChooseStageCircuitProxy chooseStageCircuitProxy = new ChooseStageCircuitProxy();
            Facade.RegisterProxy(chooseStageCircuitProxy);

        

           

            SendNotification(OrderSystemEvent.START_CIRCUIT, mainUI, OrderSystemEvent.START_CIRCUIT_MAIN);


        }
    }
}