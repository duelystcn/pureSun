
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView
{
    public class GraveyardButton : ViewBaseView
    {
        //被点击时调用方法
        public UnityAction OnPointerClick = () => { };

        // 当按钮被按下后系统自动调用此方法
        public void PointerClick()
        {
            OnPointerClick();
        }

        public void InitView()
        {

            Vector3 postion = new Vector3();
            postion.x = -420;
            postion.y = -310;
            postion.z = 0;
            this.transform.localPosition = postion;
        }

        public override void InitViewForParameter(UIControllerListMediator mediator, object body, Dictionary<string, string> parameterMap)
        {
            this.OnPointerClick = () =>
            {
                mediator.SendNotification(OperateSystemEvent.OPERATE_SYS, mediator.playerCode, OperateSystemEvent.OPERATE_SYS_GRAVEYARD_LIST_LOAD);
                
            };
            this.InitView();
        }

    }
}
