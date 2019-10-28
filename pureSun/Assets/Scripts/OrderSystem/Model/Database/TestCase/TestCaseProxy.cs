/*************************************************************************************
     * 类 名 称：       TestCaseProxy
     * 文 件 名：       TestCaseProxy
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：       代理类，TestCaseInfo.json中的TestCase信息并实例化储存到testCaseInfoMap中
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/


using Newtonsoft.Json;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assets.Scripts.OrderSystem.Model.Database.TestCase
{
    public class TestCaseProxy : Proxy
    {
        public new const string NAME = "TestCaseProxy";

        //测试用例集合
        public Dictionary<string, TestCaseInfo> testCaseInfoMap;

        public TestCaseProxy() : base(NAME)
        {
            testCaseInfoMap = new Dictionary<string, TestCaseInfo>();
            base.Data = testCaseInfoMap;
            LoadCardDbByJson();
        }
        //读取JSON文件配置
        public void LoadCardDbByJson()
        {
            string testCaseInfoStr = File.ReadAllText("Assets/Resources/Json/TestCase.json", Encoding.GetEncoding("gb2312"));
            testCaseInfoMap =
                JsonConvert.DeserializeObject<Dictionary<string, TestCaseInfo>>(testCaseInfoStr);

        }
    }
}
