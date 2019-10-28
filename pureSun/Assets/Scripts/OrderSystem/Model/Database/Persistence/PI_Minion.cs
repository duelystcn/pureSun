/*************************************************************************************
     * 类 名 称：       PI_Minion
     * 文 件 名：       PI_Minion
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        持久化类，用于读取或储存生物信息
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

namespace Assets.Scripts.OrderSystem.Model.Database.Persistence
{
    public class PI_Minion
    {
        //对应的cardCode
        public string code
        {
            get; set;
        }
        //所在横坐标
        public int x { get; set; }
        //所在竖坐标
        public int z { get; set; }
    }
}
