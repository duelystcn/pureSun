
/*************************************************************************************
     * 类 名 称：       TestCaseInfo
     * 文 件 名：       TestCaseInfo
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        测试样例类，用于储存测试案例
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/



using Assets.Scripts.OrderSystem.Model.Database.Persistence;

namespace Assets.Scripts.OrderSystem.Model.Database.TestCase
{
    public class TestCaseInfo
    {
        //code
        public string name { get; set; }
        //己方
        public PI_Player myselfPlayer { get; set; }
        //敌人
        public PI_Player enemyPlayer { get; set; }
        //备注描述
        public string description { get; set; }
        //使用的模式
        public string gameModelName { get; set; }
        //从什么阶段开始
        public string startStage { get; set; }
    }
}
