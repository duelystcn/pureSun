

using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Hex
{
    public class HexCellItem
    {
        public int X { get; private set; }

        public int Z { get; private set; }
        //坐标显示
        public HexCoordinates coordinates;
        //颜色
        public Color color;

        public HexCellItem(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}
