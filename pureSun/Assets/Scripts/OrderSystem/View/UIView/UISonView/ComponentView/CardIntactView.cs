

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.TraitSignComponent;
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

        public MonoBehaviour outLight;


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
            cardDescription.text = card.description;
            //Assets\Resources\Image\CardBG
            //image路径
            string path = "Image/Card/CardBG/";
            path = path + card.bgImageName;
            MonoBehaviour cardBg = UtilityHelper.FindChild<MonoBehaviour>(transform, "CardBg");
            changeImageSprite(cardBg, path);
            outLight.gameObject.SetActive(false);

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

        public void SetOutLight(bool canUse)
        {
            outLight.gameObject.SetActive(canUse);
        }

    }
}
