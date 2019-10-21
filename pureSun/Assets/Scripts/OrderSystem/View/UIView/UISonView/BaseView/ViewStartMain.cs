

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using OrderSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ViewStartMain:ViewBaseView
    {
        [SerializeField] private Button btnStart;
        [SerializeField] private Button testMapStart;
        [SerializeField] private Button testCaseStart;


        [SerializeField] private GameObject objNoticeBG;
        public UnityAction StartCompleteGameUnityAction;
        public UnityAction StartTestMapUnityAction;
        public UnityAction StartTestCaseUnityAction;

        protected override void InitUIObjects()
        {
            base.InitUIObjects();

            btnStart.onClick.AddListener(StartCompleteGame);
            testMapStart.onClick.AddListener(StartTestMap);
            testCaseStart.onClick.AddListener(StartTestCase);

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
        private void StartTestCase()
        {
            StartTestCaseUnityAction();
            this.gameObject.SetActive(false);
        }

        public override void InitViewForParameter(UIControllerListMediator mediator, object body) {
            this.StartCompleteGameUnityAction += () =>
            {
                mediator.SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.StartMain), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                mediator.SendNotification(OrderSystemEvent.START_CIRCUIT, null, OrderSystemEvent.START_CIRCUIT_START);
            };
            this.StartTestMapUnityAction += () =>
            {
                mediator.SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.StartMain), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                mediator.SendNotification(OrderSystemEvent.START_CIRCUIT, null, OrderSystemEvent.START_CIRCUIT_TEST_MAP);
            };
            this.StartTestCaseUnityAction += () =>
            {
                mediator.SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.StartMain), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                mediator.SendNotification(OrderSystemEvent.START_CIRCUIT, null, OrderSystemEvent.START_CIRCUIT_TEST_CASE);
            };
        }
    }
}
