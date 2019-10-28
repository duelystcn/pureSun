/*************************************************************************************
     * 类 名 称：       GameModel
     * 文 件 名：       GameModel
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        实体类，储存从GameModel.json中读取到的值
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/


namespace Assets.Scripts.OrderSystem.Model.Database.GameModelInfo
{
    public class GameModel
    {
        public string code { get; set; }
        public string name { get; set; }
        //地图宽度
        public int width { get; set; }
        //地图高度
        public int height { get; set; }
        //格子类型，hex表示六边形，目前只开发了六边形
        public string cellType { get; set; }
        //牌列类型，六边形有两种排列类型
        public string arrayMode { get; set; }
        //玩家人数
        public int playerNum { get; set; }
        //回合阶段
        public string[] turnStage { get; set; }
        //玩家具体设置
        public PlayerSite[] playerSiteList { get; set; }

    }
}
