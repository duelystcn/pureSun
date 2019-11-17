

using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Hex
{
    public enum HexModelType
    {
        Source,
        Error
    }

    public class HexModelInfo
    {
        //模式名称
        public HexModelType hexModelType;

        public int width { get;  set; }
        public int height { get;  set; }
        public string arrayMode { get; set; }
        public HexModelInfo(int w,int h,string mode, HexModelType hexModelType) {
            this.hexModelType = hexModelType;
            this.width = w;
            this.height = h;
            this.arrayMode = mode;
        }
        public List<HexCoordinates> expansionVector = new List<HexCoordinates>();
    }
}
