

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Model.Hex;
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
        //是自己的还是对手的
        public bool myself = false;
        //type参数
        public Dictionary<string, string> parameterMap;
        //设置地图模式
        public HexModelInfo hexModelInfo;

       

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
            parameterMap = StringUtil.GetParameterMapForNotificationType(notification.Type);
            playerCodeNotification = parameterMap["PlayerCode"];
            notification.Type = parameterMap["NotificationType"];
            switch (notification.Name)
            {
                case OrderSystemEvent.CLINET_SYS:

                    switch (notification.Type)
                    {
                        case OrderSystemEvent.CLINET_SYS_OWNER_CHANGE:
                            playerCode  = (string)notification.Body;
                            break;
                        case OrderSystemEvent.CLINET_SYS_GMAE_MODEL_SET:
                            hexModelInfo = (HexModelInfo)notification.Body;
                            break;
                    }
                    break;
            }
            if (playerCode == playerCodeNotification)
            {
                myself = true;
            }
            else {
                myself = false;
            }
        }

        //消息序列
        public Queue<INotification> notificationQueue = new Queue<INotification>();

        //处理消息中
        public bool exceINotification = false;

        public override void HandleNotification(INotification notification)
        {
            

        }
        public void DoExceHandleNotification()
        {
            if (exceINotification == false)
            {
                //判断是否有消息需要处理
                if (notificationQueue.Count > 0)
                {
                    exceINotification = true;
                    INotification notification = notificationQueue.Dequeue();
                    ExceHandleNotification(notification);
                }
            }

        }

        public virtual void ExceHandleNotification(INotification notification)
        {


        }

    }
}
