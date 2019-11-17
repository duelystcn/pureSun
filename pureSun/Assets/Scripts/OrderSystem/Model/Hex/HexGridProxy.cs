


using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using Assets.Scripts.OrderSystem.Util;
using OrderSystem;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Hex
{
    public class HexGridProxy : Proxy
    {
        public new const string NAME = "HexGridProxy";
        public HexGridItem HexGrid
        {
            get { return (HexGridItem)base.Data; }
        }

        public HexGridProxy() : base(NAME)
        {
        }
        public void InitializeTheProxy(HexModelInfo modelInfo) {
            
            //地图大小
            HexGridItem hexGrid = new HexGridItem(modelInfo);
            //创建
            hexGrid.CreateGrid();
            base.Data = hexGrid;
        }
        //根据坐标获取一个地图格子

 


        public void UpdateCellItem(HexCellItem cellItem)
        {
            
            SendNotification(OrderSystemEvent.CHANGE_OVER, HexGrid, "CHANGEOVER");
        }

        //传入一个生物，判断出这个生物的可移动距离
        public Dictionary<HexCoordinates, HexCellItem> GetCanMoveCellByMinionCard(CardEntry minionCard, HexModelInfo modelInfo) {
            Dictionary<HexCoordinates, HexCellItem> alreadyPassedCellMap = new Dictionary<HexCoordinates, HexCellItem>();
            alreadyPassedCellMap.Add(minionCard.nowIndex, HexGrid.cellMap[minionCard.nowIndex]);
            for (int n = 0; n < minionCard.cardInfo.movingDistance; n++) {
                Dictionary<HexCoordinates, HexCellItem> oneCheckAddMap = new Dictionary<HexCoordinates, HexCellItem>();
                foreach (KeyValuePair<HexCoordinates, HexCellItem> keyValuePair in alreadyPassedCellMap)
                {
                    HexCoordinates startCoordinates = keyValuePair.Key;
                    foreach (HexCoordinates hexCoordinates in modelInfo.expansionVector)
                    {
                        HexCoordinates targetHexCoordinates = HexUtil.GetTargetHexCoordinatesByStartPointAndVector(startCoordinates, hexCoordinates);
                        if (HexGrid.cellMap.ContainsKey(targetHexCoordinates))
                        {
                            if (HexGrid.cellMap[targetHexCoordinates].inThisCellCardList.Count == 0)
                            {
                                if (!alreadyPassedCellMap.ContainsKey(targetHexCoordinates)&& !oneCheckAddMap.ContainsKey(targetHexCoordinates)) {
                                    HexGrid.cellMap[targetHexCoordinates].pathfindingLastCell = keyValuePair.Value;
                                    oneCheckAddMap.Add(targetHexCoordinates, HexGrid.cellMap[targetHexCoordinates]);
                                }
                            }
                        }
                    }

                }
                foreach (KeyValuePair<HexCoordinates, HexCellItem> keyValuePair in oneCheckAddMap) {
                    alreadyPassedCellMap.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return alreadyPassedCellMap;
        }

    }
}
