using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using OrderSystem;
using PureMVC.Interfaces;
using System.Collections.Generic;
using UnityEngine.Events;

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

        public override void HandleNotification(INotification notification)
        {
           
            if (notification.Name == HandSystemEvent.HAND_CHANGE && notification.Type == HandSystemEvent.HAND_CHANGE_ANIMATION_START)
            {
                DoExceHandleNotification();
            }
            else if (notification.Name == OrderSystemEvent.CLINET_SYS)
            {
                //客户端监听发放，不做处理
                HandleNotificationCommon(notification);
            }
            else
            {
                notificationQueue.Enqueue(notification);
                DoExceHandleNotification();
            }

        }


        //监听
        public override void ExceHandleNotification(INotification notification)
        {

            //处理公共请求
            HandleNotificationCommon(notification);
            //回调函数
            UnityAction callBack = () =>
            {
                exceINotification = false;
                SendNotification(HandSystemEvent.HAND_CHANGE, null, HandSystemEvent.HAND_CHANGE_ANIMATION_START);

            };
            bool callBackDelay = false;


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
                        //抽了一张牌
                        case HandSystemEvent.HAND_CHANGE_DRAW_ONE_CARD:
                            if (playerCode == playerCodeNotification)
                            {
                                //回调函数
                                callBack = () =>
                                {
                                    exceINotification = false;
                                    SendNotification(HandSystemEvent.HAND_CHANGE, handGridView, HandSystemEvent.HAND_CHANGE_OVER);
                                    SendNotification(UIViewSystemEvent.UI_ANIMATION_SYS, null, UIViewSystemEvent.UI_ANIMATION_SYS_ZF_OVER_START);
                                    SendNotification(HandSystemEvent.HAND_CHANGE, null, HandSystemEvent.HAND_CHANGE_ANIMATION_START);
                                };
                                callBackDelay = true;
                                HandCellItem handCellItemDraw = notification.Body as HandCellItem;
                                handGridView.PlayerDrawOneCard(handCellItemDraw, callBack);
                            }
                            else {
                                //目前还没有实现对手手牌栏的显示，直接回调
                                //回调函数
                                callBack = () =>
                                {
                                    exceINotification = false; 
                                    SendNotification(UIViewSystemEvent.UI_ANIMATION_SYS, null, UIViewSystemEvent.UI_ANIMATION_SYS_ZF_OVER_START);
                                    SendNotification(HandSystemEvent.HAND_CHANGE, null, HandSystemEvent.HAND_CHANGE_ANIMATION_START);
                                };
                                callBack();
                                callBackDelay = true;
                            }

                            break;
                        //移除一张牌
                        case HandSystemEvent.HAND_CHANGE_REMOVE_ONE_CARD:
                            HandCellItem handCellItemRemove = notification.Body as HandCellItem;
                            callBackDelay = true;
                            handGridView.PlayerRemoveOneCard(handCellItemRemove, callBack);
                            break;
                        //是否可用渲染
                        case HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE:
                            List<HandCellItem> handCells = notification.Body as List<HandCellItem>;
                            handGridView.HandChangeCanUseJudge(handCells);
                            break;
                        //是否可用渲染
                        case HandSystemEvent.HAND_CHANGE_UNCHECK_STATUS:

                            HandCellItem uncheckHandCellItem = notification.Body as HandCellItem;
                            handGridView.HandChangeUncheckHandItem(uncheckHandCellItem);
                            break;
                    }
                    break;
            }
            if (callBackDelay == false)
            {
                callBack();
            }
        }
    }
}
