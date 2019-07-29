using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    class HandGridMediator : MediatorExpand
    {
        public new const string NAME = "HandGridMediator";

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
          
            //初始化手牌区域
            //handGridView.AchieveHandGrid(handGridProxy.handGridItem);
        }
        //监听列表
        public override string[] ListNotificationInterests()
        {
            List<string> notificationList = new List<string>();
            notificationList.Add(HandSystemEvent.HAND_CHANGE);
            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();
        }
        //监听
        public override void HandleNotification(INotification notification)
        {
            //处理公共请求
            HandleNotificationCommon(notification);

            switch (notification.Name)
            {
                case HandSystemEvent.HAND_CHANGE:
                    switch (notification.Type)
                    {
                        case HandSystemEvent.HAND_CHANGE_AFFLUX:
                            HandGridItem handGridItem = notification.Body as HandGridItem;
                            if (playerCode.Equals(playerCodeNotification)) {
                                handGridView.AchieveHandGrid(handGridItem);
                                SendNotification(HandSystemEvent.HAND_CHANGE, handGridView, HandSystemEvent.HAND_CHANGE_OVER);
                            }
                            break;
                        case HandSystemEvent.HAND_CHANGE_OVER:
                            foreach (HandCellView handCellView in handGridView.handCellViews) {
                                handCellView.OnPointerDown = () =>
                                {

                                    //消息通知-进入选中手牌操作模式
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, handCellView.handCellItem, OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE);
                                    //消息通知-划线组件激活
                                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, handCellView.handCellItem, OperateSystemEvent.OPERATE_TRAIL_DRAW_START);
                                };
                                //发出鼠标移入消息
                                handCellView.OnPointerEnter = () =>
                                {
                                    //SendNotification(HandSystemEvent.HAND_CHANGE, handCellView.handCellItem, HandSystemEvent.HAND_CHANGE_POINTER_ENTER);
                                };
                                //发出鼠标移出消息
                                handCellView.OnPointerExit = () =>
                                {
                                    //SendNotification(HandSystemEvent.HAND_CHANGE, handCellView.handCellItem, HandSystemEvent.HAND_CHANGE_POINTER_EXIT);
                                };

                            }
                            break;
                        case HandSystemEvent.HAND_CHANGE_USE_OVER:
                            HandCellItem chooseHand = notification.Body as HandCellItem;
                            //移除这张牌
                            handGridView.RemoveOneHandCellViewByHandCellItem(chooseHand);
                            break;
                        //发出鼠标移入消息
                        case HandSystemEvent.HAND_CHANGE_POINTER_ENTER:
                            HandCellItem enterHand = notification.Body as HandCellItem;
                            //handGridView.OneCardMousenPointerEnter(enterHand);
                            break;
                        //发出鼠标移出消息
                        case HandSystemEvent.HAND_CHANGE_POINTER_EXIT:
                            HandCellItem exitHand = notification.Body as HandCellItem;
                            //handGridView.OneCardMousenPointerExit(exitHand);
                            break;

                    }
                    break;
            }
                    
        }
    }
}
