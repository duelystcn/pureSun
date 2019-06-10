using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionGridMediator : Mediator
    {
        public new const string NAME = "MinionGridMediator";

        private MinionGridProxy minionGridProxy = null;

        private HexGridProxy hexGridProxy = null;


        public MinionGridView minionGridView
        {
            get { return (MinionGridView)base.ViewComponent; }
        }

        public MinionGridMediator(MinionGridView minionGridView) : base(NAME, minionGridView)
        {
        }

        //注册时执行
        public override void OnRegister()
        {
            base.OnRegister();
            minionGridProxy = Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;
            if (null == minionGridProxy)
                throw new Exception(MinionGridProxy.NAME + "is null.");
            hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            //初始化生物区域
            minionGridView.AchieveMinionGrid(minionGridProxy.minionGridItem, hexGridProxy.HexGrid);
        }
        //监听
        public override string[] ListNotificationInterests()
        {
            string[] notifications = new string[1];
            notifications[0] = MinionSystemEvent.MINION_VIEW;
            return notifications;
        }
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case MinionSystemEvent.MINION_VIEW:
                    switch (notification.Type) {
                        //生物模型变更，重新加载
                        case MinionSystemEvent.MINION_VIEW_CHANGE_OVER:
                            MinionGridItem MinionGridItem =  notification.Body as MinionGridItem;
                            minionGridView.AchieveMinionGrid(minionGridProxy.minionGridItem, hexGridProxy.HexGrid);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE:
                            List<MinionCellItem> mList = notification.Body as List<MinionCellItem>;
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                       
                    }
                    break;
               

            }
        }
    }
}
