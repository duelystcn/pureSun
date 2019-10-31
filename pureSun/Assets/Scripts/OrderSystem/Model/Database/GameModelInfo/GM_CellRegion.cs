
namespace Assets.Scripts.OrderSystem.Model.Database.GameModelInfo
{
    public class GM_CellRegion
    {
        //区域的代码
        public string code { get; set; }
        //区域的名称
        public string name { get; set; }
        //区域所包含的坐标
        public GM_CellCoordinate[] regionCellList { get; set; }
    }
}
