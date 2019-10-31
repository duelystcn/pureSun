/*************************************************************************************
     * 类 名 称：       GameModel
     * 文 件 名：       GameModel
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        实体类，储存从StageSite.json中读取到的值，关于每个阶段该做什么的控制
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

namespace Assets.Scripts.OrderSystem.Model.Database.GameModelInfo
{
    public class GM_OneStageSite
    {
        //code
        public string code { get; set; }
        //名称
        public string name { get; set; }
        //需要执行的效果
        public string[] effectNeedExeList { get; set; }
        //是否自动处理
        public string automatic { get; set; }
    }
}
