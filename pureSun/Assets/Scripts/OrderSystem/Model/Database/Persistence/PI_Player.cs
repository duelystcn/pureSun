/*************************************************************************************
     * 类 名 称：       PI_Player
     * 文 件 名：       PI_Player
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        持久化类，用于读取或储存玩家信息
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

namespace Assets.Scripts.OrderSystem.Model.Database.Persistence
{
    public class PI_Player
    {
        //主要ID
        public string playerCode
        {
            get;  set;
        }
        //船只
        public string shipCardCode
        {
            get;  set;
        }
        //手牌
        public string[] handCard { get; set; }
        //卡组列表
        public string[] deckCard { get; set; }
        //当前拥有科技
        public string[] traitList
        {
            get; set;
        }
        //费用上限
        public int manaUpperLimit { get; set; }
        //可用费用
        public int manaUsable { get; set; }
        //所拥有生物，如果可以将会单独储存
        public PI_Minion[] minionList { get; set; }
    }
}
