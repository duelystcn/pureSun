using Assets.Scripts.OrderSystem.Common.UnityExpand;
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
                case HexSystemEvent.HEX_VIEW_SYS_SHOW_START:
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_SYS_SHOW);

                    break;

                //这一段代码写的比较糟糕，只用作效果展示
                //需要区分模式
                case HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL:
                    //传递的参数中有生物信息
                    OperateSystemItem operateSystemItem = notification.Body as OperateSystemItem;
                    //获取当前玩家固定可召唤区域
                    List<HexCoordinates> fixedCanCellHexList = operateSystemItem.playerItem.fixedCanCellHexList;
                    
                    //遍历渲染
                    foreach (HexCellItem hexCellItem in hexGridProxy.HexGrid.cells) {
                        if (operateSystemItem.playerItem.CheckOneCellCanCall(hexCellItem.coordinates)) {
                            hexCellItem.borderState = BorderState.CanCall;
                        }
                    }
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_OVER);
                    break;
                case HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL:
                    foreach (HexCellItem hexCellItem in hexGridProxy.HexGrid.cells)
                    {
                        if (hexCellItem.borderState == BorderState.CanCall) {
                            hexCellItem.borderState = BorderState.Normal;
                        }  
                    }
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL_OVER);
                    break;

            }
        }
    }
}
