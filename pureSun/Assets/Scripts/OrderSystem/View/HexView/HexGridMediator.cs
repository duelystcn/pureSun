using Assets.Scripts.OrderSystem.Model.Hex;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.HexView
{
    class HexGridMediator : Mediator
    {
        public new const string NAME = "HexGridMediator";
        private HexGridProxy hexGridProxy = null;
        public HexGridView hexGridView
        {
            get { return (HexGridView)base.ViewComponent; }
        }

        public HexGridMediator(HexGridView hexGridView) : base(NAME, hexGridView)
        {
            
        }
        public override void OnRegister()
        {
            base.OnRegister();
            hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            if (null == hexGridProxy)
                throw new Exception(HexGridProxy.NAME + "is null.");
            //初始化地图区域
            hexGridView.AchieveHexGrid(hexGridProxy.HexGrid);
            //给每一个单元格绑定上单击事件
            foreach (HexCellView hexCellView in hexGridView.cellViews) {
                hexCellView.OnClick += () => 
                {
                    SendNotification(OrderSystemEvent.ONCLICK,hexCellView.hexCellItem,"CLICK");
                  
                };
            }
        }

        public override string[] ListNotificationInterests()
        {
            string[] notifications = new string[2];
            notifications[0] = OrderSystemEvent.ONCLICK;
            notifications[1] = OrderSystemEvent.CHANGE_OVER;
            return notifications;
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case OrderSystemEvent.ONCLICK:
                    HexCellItem hexCellItem = notification.Body as HexCellItem;
                    //hexGridProxy.UpdateCellItem(hexCellItem);
                    break;
                case OrderSystemEvent.CHANGE_OVER:
                    HexGridItem hexGridItem = notification.Body as HexGridItem;
                    hexGridView.UpdateHexGrid(hexGridItem);
                    break;

            }
        }

        //根据坐标获取点击的格子
        public HexCellView GetHexCellViewByPosition(Vector3 position) {
            return hexGridView.GetCellByPosition(position);
        }
    }
}
