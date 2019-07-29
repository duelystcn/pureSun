

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView
{
    public class CardHeadView : ViewBaseView
    {
        public CardEntry card;
        public UnityAction OnPointerEnter;
        public UnityAction OnPointerExit;

        public void PointerEnter() {
            OnPointerEnter();
        }
        public void PointerExit()
        {
            OnPointerExit();
        }

        public void LoadCard(CardEntry card)
        {
            this.card = card;
            TextMeshProUGUI cardName = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "CardName");
            cardName.text = card.cardInfo.name;
            TextMeshProUGUI cardCost = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "CardCost");
            cardCost.text = card.cost.ToString();
            string path = "Image/Card/CardHead/" + card.bgImageName + "_head";
            changeImageSprite(this, path);
        }
    }
}
