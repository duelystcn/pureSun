using Assets.Scripts.OrderSystem.Common;
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
    class HandViewMediator : MediatorExpand
    {
        public new const string NAME = "HandGridMediator";

        public HandControlView handControlView
        {
            get { return (HandControlView)base.ViewComponent; }
        }

        public HandViewMediator(HandControlView handControlView) : base(NAME, handControlView)
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
            notificationList.Add(HandSystemEvent.HAND_VIEW_SYS);
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
                                handControlView.handGridViewMap[playerCodeNotification].AchieveHandGrid(handGridItem);               
                                SendNotification(HandSystemEvent.HAND_CHANGE, handControlView.handGridViewMap[playerCodeNotification], StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_OVER, playerCodeNotification));
                            }
                            break;
                        case HandSystemEvent.HAND_CHANGE_OVER:
                            //只有是自己的牌才会激活操作
                            if (myself)
                            {
                                foreach (HandCellView handCellView in handControlView.handGridViewMap[playerCodeNotification].handCellViews)
                                {
                                    handCellView.OnPointerDown = () =>
                                    {
                                        if (handCellView.handCellItem.canUse) {
                                            //消息通知-进入选中手牌操作模式
                                            SendNotification(OperateSystemEvent.OPERATE_SYS, handCellView.handCellItem, OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE);
                                            //消息通知-划线组件激活
                                            SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, handCellView.handCellItem, OperateSystemEvent.OPERATE_TRAIL_DRAW_START);
                                        }
                                        
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
                                //回调函数
                                callBack = () =>
                                {
                                    exceINotification = false;
                                    SendNotification(HandSystemEvent.HAND_CHANGE, handControlView.handGridViewMap[playerCodeNotification], StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_OVER, playerCodeNotification));
                                    SendNotification(UIViewSystemEvent.UI_ANIMATION_SYS, null, UIViewSystemEvent.UI_ANIMATION_SYS_ZF_OVER_START);
                                    SendNotification(HandSystemEvent.HAND_CHANGE, null, HandSystemEvent.HAND_CHANGE_ANIMATION_START);
                                };
                                callBackDelay = true;
                                HandCellItem handCellItemDraw = notification.Body as HandCellItem;
                                handControlView.handGridViewMap[playerCodeNotification].PlayerDrawOneCard(handCellItemDraw, callBack);

                            break;
                        //移除一张牌
                        case HandSystemEvent.HAND_CHANGE_REMOVE_ONE_CARD:
                            HandCellItem handCellItemRemove = notification.Body as HandCellItem;
                            callBackDelay = true;
                            handControlView.handGridViewMap[playerCodeNotification].PlayerRemoveOneCard(handCellItemRemove, callBack);
                            break;
                        //是否可用渲染
                        case HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE:
                            if (myself)
                            {
                                List<HandCellItem> handCells = notification.Body as List<HandCellItem>;
                                handControlView.handGridViewMap[playerCodeNotification].HandChangeCanUseJudge(handCells);
                            }
                            break;
                        //手牌恢复到初始状态，当使用手牌没有成功的
                        case HandSystemEvent.HAND_CHANGE_UNCHECK_STATUS:

                            HandCellItem uncheckHandCellItem = notification.Body as HandCellItem;
                            handControlView.handGridViewMap[playerCodeNotification].HandChangeUncheckHandItem(uncheckHandCellItem);
                            break;
                    }
                    break;
                case HandSystemEvent.HAND_VIEW_SYS:
                    switch (notification.Type)
                    {
                        //创建一个手牌栏
                        case HandSystemEvent.HAND_VIEW_SYS_INIT_PLAYER_CODE:
                            handControlView.CreateHandGridViewByPlayerCode(playerCodeNotification, myself);
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
