/*************************************************************************************
     * 类 名 称：       GameModelProxy
     * 文 件 名：       GameModelProxy
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        代理类，读取GameModel.json中的GameModel信息并实例化储存到gameModelInfoMap中
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Hex;
using Newtonsoft.Json;
using OrderSystem;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assets.Scripts.OrderSystem.Model.Database.GameModelInfo
{
    public class GameModelProxy : Proxy
    {
        public new const string NAME = "GameModelProxy";

        //游戏模式集合
        public Dictionary<string, GameModel> gameModelInfoMap;
        //游戏模式集合
        public Dictionary<string, GM_OneStageSite> stageSiteMap;
        //当前游戏模式
        public GameModel gameModelNow = new GameModel();
        //当前坐标模式
        public HexModelInfo hexModelInfoNow = null;

        //设置当前游戏模式
        public void setGameModelNow (string name){
            //先默认是安多尔模式
            gameModelNow = this.gameModelInfoMap[name];
            UtilityLog.Log("当前游戏模式：" + gameModelNow.name,LogUtType.Other);

            //地图模式
            string arrayMode = gameModelNow.arrayMode;
            //地图大小
            int height = gameModelNow.height;
            int width = gameModelNow.width;

            hexModelInfoNow = new HexModelInfo(width, height, arrayMode, HexModelType.Source);
            SendNotification(OrderSystemEvent.CLINET_SYS, hexModelInfoNow, OrderSystemEvent.CLINET_SYS_GMAE_MODEL_SET);
        }

        public GameModelProxy() : base(NAME)
        {
            gameModelInfoMap = new Dictionary<string, GameModel>();
            base.Data = gameModelInfoMap;
            LoadCardDbByJson();
        }
        //读取JSON文件配置
        public void LoadCardDbByJson()
        {
            string gameModelInfoStr = File.ReadAllText("Assets/Resources/Json/GameModel.json", Encoding.GetEncoding("gb2312"));
            gameModelInfoMap =
                JsonConvert.DeserializeObject<Dictionary<string, GameModel>>(gameModelInfoStr);
            string stageSiteStr = File.ReadAllText("Assets/Resources/Json/StageSite.json", Encoding.GetEncoding("gb2312"));
            stageSiteMap =
                JsonConvert.DeserializeObject<Dictionary<string, GM_OneStageSite>>(stageSiteStr);

        }
    }
}
