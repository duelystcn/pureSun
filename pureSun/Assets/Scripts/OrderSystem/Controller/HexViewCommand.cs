using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
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
            GameModelProxy gameModelProxy =
               Facade.RetrieveProxy(GameModelProxy.NAME) as GameModelProxy;
            switch (notification.Type)
            {
                case HexSystemEvent.HEX_VIEW_SYS_SHOW_START:
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_SYS_SHOW);
                    break;

                case HexSystemEvent.HEX_VIEW_SYS_CARD_CHANGE_GAME_CONTAINER_TYPE:
                    CardEntry cardEntryChangeGCType = notification.Body as CardEntry;
                    if (cardEntryChangeGCType.gameContainerType == "CardBattlefield")
                    {
                        hexGridProxy.HexGrid.cellMap[cardEntryChangeGCType.nowIndex].inThisCellCardList.Add(cardEntryChangeGCType);
                        return;
                    }
                    if (cardEntryChangeGCType.lastGameContainerType == "CardBattlefield")
                    {
                        hexGridProxy.HexGrid.cellMap[cardEntryChangeGCType.nowIndex].inThisCellCardList.Remove(cardEntryChangeGCType);
                    }

                    break;
                //这一段代码写的比较糟糕，只用作效果展示
                //需要区分模式,渲染出可召唤区域
                case HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL:
                    //传递的参数中有生物信息
                    OperateSystemItem operateSystemItemCanCall = notification.Body as OperateSystemItem;
                    //获取当前玩家固定可召唤区域
                    List<HexCoordinates> fixedCanCellHexList = operateSystemItemCanCall.playerItem.fixedCanCallHexList;

                    //遍历渲染
                    foreach (KeyValuePair<HexCoordinates, HexCellItem> keyValuePair in hexGridProxy.HexGrid.cellMap) {
                        if (operateSystemItemCanCall.playerItem.CheckOneCellCanCall(keyValuePair.Key))
                        {
                            keyValuePair.Value.borderState = BorderState.CanCall;
                        }
                    }
                     
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_OVER);
                    break;
                case HexSystemEvent.HEX_VIEW_RENDER_CAN_MOVE_AND_ATK:
                    //传递的参数中有生物信息
                    OperateSystemItem operateSystemItemCanMoveAndAttack = notification.Body as OperateSystemItem;
                    Dictionary<HexCoordinates, HexCellItem> alreadyPassedCellMap = hexGridProxy.GetCanMoveCellByMinionCard(operateSystemItemCanMoveAndAttack.onChooseCardEntry, gameModelProxy.hexModelInfoNow);
                    //遍历渲染
                    foreach (KeyValuePair<HexCoordinates, HexCellItem> keyValuePair in alreadyPassedCellMap)
                    {
                      
                        keyValuePair.Value.borderState = BorderState.CanCall;
                    }
                    operateSystemItemCanMoveAndAttack.onChooseCardEntry.canBeMovedCellMap = alreadyPassedCellMap;
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_OVER);
                    break;
                case HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL:
                    //遍历渲染
                    foreach (KeyValuePair<HexCoordinates, HexCellItem> keyValuePair in hexGridProxy.HexGrid.cellMap)
                    {
                        if (keyValuePair.Value.borderState == BorderState.CanCall)
                        {
                            keyValuePair.Value.borderState = BorderState.Normal;
                        }
                    }
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, hexGridProxy.HexGrid, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL_OVER);
                    break;

            }
        }
    }
}
