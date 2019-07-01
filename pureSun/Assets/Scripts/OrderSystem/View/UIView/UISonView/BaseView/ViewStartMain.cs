

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ViewStartMain:ViewBaseView
    {
        [SerializeField] private Button btnStart;
        [SerializeField] private GameObject objNoticeBG;
        public UnityAction unityAction;
        protected override void InitUIObjects()
        {
            base.InitUIObjects();

            btnStart.onClick.AddListener(ClickStart);
            //btnStart.gameObject.SetActive(false);
            //objNoticeBG.gameObject.SetActive(false);
        }
        private void ClickStart()
        {
            unityAction();
            btnStart.gameObject.SetActive(false);
            objNoticeBG.SetActive(false);
        }
    }
}
