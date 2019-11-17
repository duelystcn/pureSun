

using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Hex
{
    public enum BorderState {
        Normal,
        CanCall
    }

    public class HexCellItem
    {
        public int X { get; private set; }

        public int Z { get; private set; }
        //坐标显示
        public HexCoordinates coordinates;
        //边框状态
        public BorderState borderState;

        public HexCellItem(int x, int z)
        {
            X = x;
            Z = z;
        }
        //棋盘上这个格子所放置的卡
        public List<CardEntry> inThisCellCardList = new List<CardEntry>();

        //用于寻路追朔
        public HexCellItem pathfindingLastCell ;


    }
}
