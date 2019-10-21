using Assets.Scripts.OrderSystem.Model.Database.TestCase;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.TestCase;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class TestCaseList :  MonoBehaviour
    {
        public OneTestCase oneTestCasePrefab;
        public List<OneTestCase> oneTestCaseList;

        public void addTestCase(List<TestCaseInfo> testCaseInfoList)
        {
            for (int i = 0; i < testCaseInfoList.Count; i++)
            {
                OneTestCase oneTestCase = Instantiate<OneTestCase>(oneTestCasePrefab);
                Vector3 position = new Vector3();
                oneTestCase.transform.SetParent(transform, false);
                oneTestCase.transform.localPosition = position;
                oneTestCaseList.Add(oneTestCase);
                oneTestCase.LoadTestCaseInfo(testCaseInfoList[i],i);
            }
        }
    }
}
