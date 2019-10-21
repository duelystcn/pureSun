

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.TestCase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.TestCase
{
    public class OneTestCase : MonoBehaviour
    {
        public UnityAction OnClick = null;

        public TestCaseInfo testCaseInfo;

        public void PointerClick()
        {
            if (OnClick != null)
            {
                OnClick();
            }
        }

        public void LoadTestCaseInfo(TestCaseInfo testCaseInfo,int num) {
            this.testCaseInfo = testCaseInfo;
            TextMeshProUGUI Rownum = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "Rownum");
            Rownum.text = num.ToString();
            TextMeshProUGUI Description = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "Description");
            Description.text = testCaseInfo.description;
          
        }
    }
}
