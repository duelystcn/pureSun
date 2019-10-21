

using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class OneCardAllInfo : ViewBaseView
    {
        public CardIntactView cardIntactView;

        public void LoadCardInfo(CardEntry cardEntry) {
            cardIntactView.LoadCard(cardEntry,true);

        }
        public override void InitViewForParameter(UIControllerListMediator mediator, object body)
        {
            CardEntry cardEntry = body as CardEntry;
            this.LoadCardInfo(cardEntry);
        }

    }
}
