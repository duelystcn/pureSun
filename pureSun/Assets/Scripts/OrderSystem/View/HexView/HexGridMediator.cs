using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hex;
using OrderSystem;
using PureMVC.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.HexView
{
    class HexGridMediator : MediatorExpand
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
            List<string> notificationList = new List<string>();
            notificationList.Add(HexSystemEvent.HEX_VIEW_SYS);
          
            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();

          
        }

        public override void HandleNotification(INotification notification)
        {
            //处理公共请求
            HandleNotificationCommon(notification);
            switch (notification.Name)
            {
                case HexSystemEvent.HEX_VIEW_SYS:
                    switch (notification.Type) {
                        case HexSystemEvent.HEX_VIEW_SYS_SHOW:
                            HexGridItem hexGrid = notification.Body as HexGridItem;
                            //初始化地图区域
                            hexGridView.AchieveHexGrid(hexGrid);
                            //给每一个单元格绑定上单击事件
                            foreach (KeyValuePair<HexCoordinates, HexCellView> keyValuePair in hexGridView.cellViewMap)
                            {
                                keyValuePair.Value.OnClick += () =>
                                {
                                    SendNotification(OrderSystemEvent.ONCLICK, keyValuePair.Value.hexCellItem, "CLICK");

                                };
                            }
                            break;
                        case HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_OVER:
                            HexGridItem hexGridItem = notification.Body as HexGridItem;
                            hexGridView.UpdateHexGrid(hexGridItem);
                            break;
                        case HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL_OVER:
                            HexGridItem hexGridItemCancel = notification.Body as HexGridItem;
                            hexGridView.UpdateHexGrid(hexGridItemCancel);
                            break;
                    }
                    break;
               

            }
        }

        //根据坐标获取点击的格子
        public HexCellView GetHexCellViewByPosition(Vector3 position) {
            return hexGridView.GetCellByPosition(position);
        }
    }
}
