


using OrderSystem;
using PureMVC.Patterns.Proxy;

namespace Assets.Scripts.OrderSystem.Model.Hex
{
    public class HexGridProxy : Proxy
    {
        public new const string NAME = "HexGridProxy";
        public HexGridItem HexGrid
        {
            get { return (HexGridItem)base.Data; }
        }

        public HexGridProxy(HexModelInfo modelInfo) : base(NAME)
        {
            //地图大小
            HexGridItem hexGrid = new HexGridItem(modelInfo);
            //创建
            hexGrid.CreateGrid();
            base.Data = hexGrid;
        }
        public void UpdateCellItem(HexCellItem cellItem)
        {
            for (int i = 0; i < HexGrid.cells.Length; i++)
            {
                if (HexGrid.cells[i].X.Equals(cellItem.X)&& HexGrid.cells[i].Z.Equals(cellItem.Z))
                {
                   
                    break;
                }
            }
            SendNotification(OrderSystemEvent.CHANGE_OVER, HexGrid, "CHANGEOVER");
        }

    }
}
