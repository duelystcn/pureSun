


using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.GraveyardListComponent;
using OrderSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class GraveyardListView: ViewBaseView
    {
        public GraveyardListOne graveyardListOne;

        public GraveyardListExit graveyardListExit;

        public override void InitViewForParameter(UIControllerListMediator mediator, object body, Dictionary<string, string> parameterMap)
        {
            GameContainerItem cardGraveyard = body as GameContainerItem;
            graveyardListOne.InitView(cardGraveyard);
            this.graveyardListExit.OnPointerClick = () => {
                //关闭墓地页面
                mediator.SendNotification(
                    UIViewSystemEvent.UI_VIEW_CURRENT,
                    UIViewConfig.getNameStrByUIViewName(UIViewName.GraveyardListView),
                    StringUtil.GetNTByNotificationTypeAndUIViewNameAndMaskLayer(
                        UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW,
                        UIViewConfig.getNameStrByUIViewName(UIViewName.GraveyardListView),
                        "N"
                    )
                );

            };
        }
    }
}
