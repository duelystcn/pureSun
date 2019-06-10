using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class HexViewCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            HexGridProxy hexGridProxy =
                      Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            switch (notification.Type)
            {
                //这一段代码写的比较糟糕，只用作效果展示
                //需要区分模式
                case HexSystemEvent.HEX_VIEW_SYS_CHANGE:
                    //传递的参数中有生物信息
                    OperateSystemItem operateSystemItem = notification.Body as OperateSystemItem;
                    //获取当前玩家虚拟坐标
                    HexCoordinates hexCoordinates = operateSystemItem.playerItem.hexCoordinates;
                    int row = 999;
                    if (hexCoordinates.Z < 0) {
                        row = hexCoordinates.Z + 1;
                    }
                    if (hexCoordinates.Z > 0) {
                        row = hexCoordinates.Z - 1 ;
                    }
                    //遍历渲染
                    foreach (HexCellItem hexCellItem in hexGridProxy.HexGrid.cells) {
                        if (hexCellItem.coordinates.Z + Mathf.RoundToInt(hexCellItem.coordinates.X/2) == row) {
                            hexCellItem.color = hexGridProxy.HexGrid.highlightColor;
                        }
                    }
                    SendNotification(OrderSystemEvent.CHANGE_OVER, hexGridProxy.HexGrid, "CHANGEOVER");
                    break;
                case HexSystemEvent.HEX_VIEW_SYS_CLOSE_HIGHLIGHT:
                    foreach (HexCellItem hexCellItem in hexGridProxy.HexGrid.cells)
                    {
                        if (hexCellItem.color == hexGridProxy.HexGrid.highlightColor) {
                            hexCellItem.color = hexGridProxy.HexGrid.defaultColor;
                        }  
                    }
                    SendNotification(OrderSystemEvent.CHANGE_OVER, hexGridProxy.HexGrid, "CHANGEOVER");
                    break;

            }
        }
    }
}
