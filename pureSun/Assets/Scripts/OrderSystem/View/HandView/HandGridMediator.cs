using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hand;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    class HandGridMediator : Mediator
    {
        public new const string NAME = "HandGridMediator";
        //手牌区代理
        private HandGridProxy handGridProxy = null;

        public HandGridView handGridView
        {
            get { return (HandGridView)base.ViewComponent; }
        }

        public HandGridMediator(HandGridView handGridView) : base(NAME, handGridView)
        {
            
        }
        //注册时执行
        public override void OnRegister()
        {
            base.OnRegister();
            handGridProxy = Facade.RetrieveProxy(HandGridProxy.NAME) as HandGridProxy;
            if (null == handGridProxy)
                throw new Exception(HandGridProxy.NAME + "is null.");
            //初始化手牌区域
            handGridView.AchieveHandGrid(handGridProxy.handGridItem);
        }
        //监听列表
        public override string[] ListNotificationInterests()
        {
            string[] notifications = new string[1];
            notifications[0] = HandSystemEvent.HAND_CHANGE;
            return notifications;
        }
        //监听
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case HandSystemEvent.HAND_CHANGE:
                    switch (notification.Type)
                    {
                        case HandSystemEvent.HAND_CHANGE_AFFLUX:
                            HandGridItem handGridItem = notification.Body as HandGridItem;
                            handGridView.AchieveHandGrid(handGridItem);
                            SendNotification(HandSystemEvent.HAND_CHANGE, handGridView, HandSystemEvent.HAND_CHANGE_OVER);
                            break;
                        case HandSystemEvent.HAND_CHANGE_OVER:
                            foreach (HandCellView handCellView in handGridView.handCellViews) {
                                handCellView.OnChoose += () =>
                                {
                                    //消息通知-进入选中手牌操作模式
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, handCellView.handCellItem, OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE);
                                    //消息通知-划线组件激活
                                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, handCellView.handCellItem, OperateSystemEvent.OPERATE_TRAIL_DRAW_START);
                                };
                            }
                            break;
                        case HandSystemEvent.HAND_CHANGE_USE_OVER:
                            HandCellItem chooseHand = notification.Body as HandCellItem;
                            //移除这张牌
                            handGridView.RemoveOneHandCellViewByHandCellItem(chooseHand);
                            break;
                    }
                    break;
            }
                    
        }
    }
}
