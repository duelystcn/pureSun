using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionGridMediator : MediatorExpand
    {
        public new const string NAME = "MinionGridMediator";


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
            hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
        }
        //监听
        public override string[] ListNotificationInterests()
        {
            List<string> notificationList = new List<string>();
            notificationList.Add(MinionSystemEvent.MINION_VIEW);
            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();
        }
        public override void HandleNotification(INotification notification)
        {
            //处理公共请求
            HandleNotificationCommon(notification);
            List<MinionCellItem> mList = new List<MinionCellItem>();
            switch (notification.Name)
            {
                case MinionSystemEvent.MINION_VIEW:
                    switch (notification.Type) {
                        //生物模型变更，重新加载
                        case MinionSystemEvent.MINION_VIEW_CHANGE_OVER:
                            MinionGridItem minionGridItem =  notification.Body as MinionGridItem;
                            minionGridView.AchieveMinionGrid(minionGridItem, hexGridProxy.HexGrid);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE:
                            mList = notification.Body as List<MinionCellItem>;
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINION_CHANGE_ATK:
                            MinionCellItem minionCellItemAtk = notification.Body as MinionCellItem;
                            mList.Add(minionCellItemAtk);
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINION_CHANGE_DEF:
                            MinionCellItem minionCellItemDef = notification.Body as MinionCellItem;
                            mList.Add(minionCellItemDef);
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            UtilityLog.Log("效果执行结束");
                            break;
                    }
                    break;
               

            }
        }
    }
}
