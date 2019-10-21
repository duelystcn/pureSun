

using Assets.Scripts.OrderSystem.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView
{
    public class NextTurnButton : ViewBaseView
    {
        public MonoBehaviour nextButtonImg;

        //原始边框
        public Material NextTurnButtonNormal;
        //外发光边框
        public Material NextTurnButtonOutLine;
        //被点击时调用方法
        public UnityAction OnPointerClick = () => { };


        public void InitView()
        {

            Vector3 postion = new Vector3();
            postion.x = 500;
            postion.y = -280;
            postion.z = 0;
            this.transform.localPosition = postion;
            nextButtonImg.gameObject.SetActive(false);
        }
        public void ShowButton() {
            nextButtonImg.gameObject.SetActive(true);
        }

        public void HideButton()
        {
            nextButtonImg.gameObject.SetActive(false);
        }

        public void PointerEnter()
        {
            Image image = nextButtonImg.transform.GetComponent<Image>();
            image.material = NextTurnButtonOutLine;
        }
        public void PointerExit()
        {
            Image image = nextButtonImg.transform.GetComponent<Image>();
            image.material = NextTurnButtonNormal;
        }
        // 当按钮被按下后系统自动调用此方法
        public void PointerClick()
        {
            OnPointerClick();
        }

        public override void InitViewForParameter(UIControllerListMediator mediator, object body)
        {
            this.OnPointerClick = () =>
            {
                mediator.SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE);
                this.HideButton();
            };
            this.InitView();
        }

    }
}
