

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ViewStartMain:ViewBaseView
    {
        [SerializeField] private Button btnStart;
        [SerializeField] private Button testMapStart;


        [SerializeField] private GameObject objNoticeBG;
        public UnityAction StartCompleteGameUnityAction;
        public UnityAction StartTestMapUnityAction;

        protected override void InitUIObjects()
        {
            base.InitUIObjects();

            btnStart.onClick.AddListener(StartCompleteGame);
            testMapStart.onClick.AddListener(StartTestMap);

            //btnStart.gameObject.SetActive(false);
            //objNoticeBG.gameObject.SetActive(false);
        }
        private void StartCompleteGame()
        {
            StartCompleteGameUnityAction();
            this.gameObject.SetActive(false);
        }
        private void StartTestMap() {
            StartTestMapUnityAction();
            this.gameObject.SetActive(false);
        }
    }
}
