using Assets.Scripts.OrderSystem.Model.Database.TestCase;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.TestCase;
using OrderSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class TestCaseView : ViewBaseView
    {
        public TestCaseList testCaseList;

        public void addTestCase(List<TestCaseInfo> testCaseInfoList)
        {
            testCaseList.addTestCase(testCaseInfoList);
        }
        public override void InitViewForParameter(UIControllerListMediator mediator, object body)
        {
            List<TestCaseInfo> testCaseInfoList = body as List<TestCaseInfo>;
            this.addTestCase(testCaseInfoList);
            foreach (OneTestCase oneTestCase in this.testCaseList.oneTestCaseList)
            {
                oneTestCase.OnClick = () =>
                {
                    mediator.SendNotification(OrderSystemEvent.START_CIRCUIT, oneTestCase.testCaseInfo, OrderSystemEvent.START_CIRCUIT_TEST_CASE_START_ONE);
                };
            };
        }
    }
}
