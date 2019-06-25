
/*=========================================
* Author: Administrator
* DateTime:2017/6/20 18:29:33
* Description:$safeprojectname$
==========================================*/

using System;
using Assets.Scripts.OrderSystem.Model.Circuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Hand;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.SpecialOperateView.ChooseView;
using Assets.Scripts.OrderSystem.View.CircuitView;
using Assets.Scripts.OrderSystem.View.HandView;
using Assets.Scripts.OrderSystem.View.HexView;
using Assets.Scripts.OrderSystem.View.MinionView;
using Assets.Scripts.OrderSystem.View.OperateSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;
using Assets.Scripts.OrderSystem.View.UIView;
/**
* 程序启动，初始化 
*/
namespace OrderSystem
{
    internal class StartUpCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            //地图模式
            string arrayMode = HexMetrics.MODE_HORIZ;
            //地图大小
            int height = 4;
            int width = 6;

            HexModelInfo modelInfo = new HexModelInfo(width,height,arrayMode);

            MainUI mainUI = notification.Body as MainUI;
            if (null == mainUI)
                throw new Exception("程序启动失败..");

            //数据信息代理
            CardDbProxy cardDbProxy = new CardDbProxy();
            Facade.RegisterProxy(cardDbProxy);
            EffectInfoProxy effectInfoProxy = new EffectInfoProxy();
            Facade.RegisterProxy(effectInfoProxy);

            //UI层代理
            UIControllerListMediator uIControllerListMediator = new UIControllerListMediator(mainUI.UIControllerListView);
            Facade.RegisterMediator(uIControllerListMediator);


            //地图代理(需要放在操作层之前)
            HexGridProxy hexGridProxy = new HexGridProxy(modelInfo);
            Facade.RegisterProxy(hexGridProxy);
            HexGridMediator hexGridMediator = new HexGridMediator(mainUI.HexGridView);
            Facade.RegisterMediator(hexGridMediator);

           

            //生物层代理
            MinionGridProxy minionGridProxy = new MinionGridProxy();
            Facade.RegisterProxy(minionGridProxy);
            MinionGridMediator minionGridMediator = new MinionGridMediator(mainUI.minionGridView);
            Facade.RegisterMediator(minionGridMediator);

            //玩家组代理
            PlayerGroupProxy playerGroupProxy = new PlayerGroupProxy();
            Facade.RegisterProxy(playerGroupProxy);

            //进程代理
            CircuitProxy circuitProxy = new CircuitProxy();
            Facade.RegisterProxy(circuitProxy);
            CircuitMediator circuitMediator = new CircuitMediator(mainUI.circuitButton);
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
            HandGridProxy handGridProxy = new HandGridProxy();
            Facade.RegisterProxy(handGridProxy);
            HandGridMediator handGridMediator = new HandGridMediator(mainUI.HandGridView);
            Facade.RegisterMediator(handGridMediator);

            SendNotification(OrderSystemEvent.START_CIRCUIT, mainUI,"");


        }
    }
}