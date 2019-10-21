
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.TraitSignComponent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.HandView
{

    public class HandCellInstance : ViewBaseView
    {
        public Image image;//获取图片组件，设置图片
        public Sprite[] img;
        public TraitSignList traitSignList;
        public MonoBehaviour handOutLight;
        /// 设置卡牌图片 
        public void SetImage()
        {
            image.sprite = img[0];
        }
        public void LoadTraitList(string[] traitdemand)
        {
            traitSignList.LoadTraitList(traitdemand);
        }

        public void LoadCard(CardEntry cardEntry, bool isPositive)
        {
            handOutLight.gameObject.SetActive(false);
            if (isPositive)
            {
                TextMeshProUGUI cardName = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "CardName");
                cardName.text = cardEntry.name;
                TextMeshProUGUI cardCost = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "CardCost");
                cardCost.text = cardEntry.cost.ToString();
                string path = "Image/Hand/" + cardEntry.bgImageName + "_hand";
                MonoBehaviour handBg = UtilityHelper.FindChild<MonoBehaviour>(transform, "HandBg");
                changeImageSprite(handBg, path);
                LoadTraitList(cardEntry.traitdemand);
            }
            else {

                string path = "Image/Hand/cardback_hand";
                MonoBehaviour handBg = UtilityHelper.FindChild<MonoBehaviour>(transform, "HandBg");
                changeImageSprite(handBg, path);
            }
        }
        public void SetOutLight(bool canUse) {
            handOutLight.gameObject.SetActive(canUse);
        }
    }
}
