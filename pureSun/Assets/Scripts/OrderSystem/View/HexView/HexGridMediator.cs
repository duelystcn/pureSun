using Assets.Scripts.OrderSystem.Event;
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
        }

        public override string[] ListNotificationInterests()
        {
            string[] notifications = new string[2];
            notifications[0] = HexSystemEvent.HEX_VIEW_SYS;
            notifications[1] = OrderSystemEvent.CHANGE_OVER;
            return notifications;
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case HexSystemEvent.HEX_VIEW_SYS:
                    switch (notification.Type) {
                        case HexSystemEvent.HEX_VIEW_SYS_SHOW:
                            HexGridItem hexGrid = notification.Body as HexGridItem;
                            //初始化地图区域
                            hexGridView.AchieveHexGrid(hexGrid);
                            //给每一个单元格绑定上单击事件
                            foreach (HexCellView hexCellView in hexGridView.cellViews)
                            {
                                hexCellView.OnClick += () =>
                                {
                                    SendNotification(OrderSystemEvent.ONCLICK, hexCellView.hexCellItem, "CLICK");

                                };
                            }
                            break;
                    }
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
