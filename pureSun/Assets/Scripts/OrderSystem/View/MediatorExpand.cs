﻿

using Assets.Scripts.OrderSystem.Common;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.View
{
    public class MediatorExpand : Mediator
    {
        public static string NAME = "MediatorExpand";
        //当前视图所属者
        public string playerCode;
        //当前消息所属者
        public string playerCodeNotification;

        public MediatorExpand(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
        {
           
        }
        //在这里可以添加所有界面需要监听的消息
        public void AddCommonNotificationInterests(List<string> notificationList) {
            notificationList.Add(OrderSystemEvent.CLINET_SYS);
        }
        //在这里可以添加所有界面需要处理的信息
        public void HandleNotificationCommon(INotification notification)
        {
            playerCodeNotification = StringUtil.GetPlayerCodeForNP(notification.Type);
            notification.Type = StringUtil.GetNotificationTypeForNP(notification.Type);
            switch (notification.Name)
            {
                case OrderSystemEvent.CLINET_SYS:

                    switch (notification.Type)
                    {
                        case OrderSystemEvent.CLINET_SYS_OWNER_CHANGE:
                            playerCode  = (string)notification.Body;
                            break;
                    }
                    break;
            }
        }
    }
}
