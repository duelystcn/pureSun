
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Hex
{   
    /*
     * 这是地图格子的实体类
     */
    public class HexGridItem
    {
       // public HexCellItem[] cells { get; private set; }

        public Dictionary<HexCoordinates, HexCellItem> cellMap = new Dictionary<HexCoordinates, HexCellItem>();
        //地图模式
        public HexModelInfo modelInfo
        {
            get; private set;
        }
        public HexGridItem(HexModelInfo modelInfo)
        {
            this.modelInfo = modelInfo;
        }
        //创建
        public void CreateGrid() {
            for (int z = 0, i = 0; z < modelInfo.height; z++)
            {
                for (int x = 0; x < modelInfo.width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        }
        //创建
        void CreateCell(int x, int z, int i)
        {
            HexCellItem hexCellItem = new HexCellItem(x, z);
            hexCellItem.borderState = BorderState.Normal;
            hexCellItem.coordinates = HexCoordinates.FromOffsetCoordinates(x, z, modelInfo.arrayMode);
            cellMap.Add(hexCellItem.coordinates,hexCellItem);

        }
    }
}
