

using Assets.Scripts.OrderSystem.Common.UnityExpand;
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
