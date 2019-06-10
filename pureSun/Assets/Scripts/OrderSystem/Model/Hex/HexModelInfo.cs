

namespace Assets.Scripts.OrderSystem.Model.Hex
{
    public class HexModelInfo
    {
        public int width { get;  set; }
        public int height { get;  set; }
        public string arrayMode { get; set; }
        public HexModelInfo(int w,int h,string mode) {
            this.width = w;
            this.height = h;
            this.arrayMode = mode;
        }

    }
}
