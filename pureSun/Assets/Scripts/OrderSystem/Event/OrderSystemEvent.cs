
/*=========================================
* Author: Administrator
* DateTime:2017/6/20 18:28:40
* Description:$safeprojectname$
==========================================*/

namespace OrderSystem
{
    public class OrderSystemEvent
    {
        /// <summary>
        /// 启动
        /// </summary>
        public const string STARTUP = "StartUp";
        /// <summary>
        /// 开始流程
        /// </summary>
        public const string START_CIRCUIT = "StartCircuit";

        public const string START_CIRCUIT_MAIN = "StartCircuitMain";

        public const string START_CIRCUIT_START = "StartCircuitStart";

        public const string START_CIRCUIT_TEST_MAP = "StartCircuitTestMap";
        /// <summary>
        /// 选择测试用例
        /// </summary>
        public const string START_CIRCUIT_TEST_CASE = "StartCircuitTestCase";
        /// <summary>
        /// 进入一个测试用例
        /// </summary>
        public const string START_CIRCUIT_TEST_CASE_START_ONE = "StartCircuitTestCaseStartOne";









        /// <summary>
        /// 被点击
        /// </summary>
        public const string ONCLICK = "OnClick";
        /// <summary>
        /// 变更完毕
        /// </summary>
        public const string CHANGE_OVER = "changeOver";

        /// <summary>
        /// 客户端所属权
        /// </summary>
        public const string CLINET_SYS = "clientSys";
        /// <summary>
        /// 客户端所属权
        /// </summary>
        public const string CLINET_SYS_OWNER_CHANGE = "clientSysOwnerChange";


    }
}