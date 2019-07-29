

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView
{
    public class CardIntactView : ViewBaseView
    {
        public CardEntry card;
        public UnityAction OnClick = null;

        public TraitSignList traitSignListPrefab;
        private TraitSignList traitSignList;

        public Material outLight;

        public void PointerClick() {
            if (OnClick != null) {
                OnClick();
            }
           
        }
        public void LoadCard(CardEntry card) {
            this.card = card;
            LoadTraitList(card.traitdemand);
            TextMeshProUGUI cardName = UtilityHelper.FindChild<TextMeshProUGUI>(transform, "CardName");
            cardName.text = card.cardInfo.name;
            TextMeshProUGUI cardCost = UtilityHelper.FindChild<TextMeshProUGUI>(transform, "CardCost");
            cardCost.text = card.cost.ToString();
            TextMeshProUGUI cardDescription = UtilityHelper.FindChild<TextMeshProUGUI>(transform, "CardDescription");
            cardDescription.text = card.cardInfo.description;
            //Assets\Resources\Image\CardBG
            //image路径
            string path = "Image/Card/CardBG/";
            path = path + card.bgImageName;
            changeImageSprite(this, path);
            Image imageComponent = this.GetComponent<Image>();
            if (card.isBuyed == false)
            {
                imageComponent.material = null;
            }
            else
            {
                imageComponent.material = outLight;
            }
        }

        public void LoadTraitList(string[] traitdemand) {
            if (traitSignList == null) {
                traitSignList = Instantiate<TraitSignList>(traitSignListPrefab);
                Vector3 position = new Vector3();
                position.x = -35;
                traitSignList.transform.SetParent(transform, false);
                traitSignList.transform.localPosition = position;
            }
            traitSignList.LoadTraitList(traitdemand);
        }

    }
}
