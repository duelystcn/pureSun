using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ViewChooseStage:ViewBaseView
    {
        public CardIntactView cardIntactViewPrefab;

        public List<CardIntactView> cardIntactViews = new List<CardIntactView>();
        public void LoadCardInfoList(List<CardInfo> shipCardList) {
            List<CardEntry> shipCardEntryList = new List<CardEntry>();
            for (int i=0;i<shipCardList.Count;i++) {
                CardEntry card = new CardEntry();
                card.cardInfo = shipCardList[i];
                card.InitializeByCardInfo(shipCardList[i]);
                shipCardEntryList.Add(card);
            }
            LoadCardEntryList(shipCardEntryList);

        }
        public void LoadCardEntryList(List<CardEntry> shipCardList)
        {
            for (int i = 0; i < shipCardList.Count; i++)
            {
                CardEntry card = shipCardList[i];
                CardIntactView cardIntactView = null;
                bool isAdd = true;
                if (i < cardIntactViews.Count)
                {
                    cardIntactView = cardIntactViews[i];
                    isAdd = false;
                }
                else
                {
                    cardIntactView = Instantiate<CardIntactView>(cardIntactViewPrefab);
                    Vector3 position = new Vector3();
                    cardIntactView.transform.SetParent(transform, false);
                    cardIntactView.transform.localPosition = position;

                }

                TextMeshProUGUI cardName = UtilityHelper.FindChild<TextMeshProUGUI>(cardIntactView.transform, "CardName");
                cardName.text = card.cardInfo.name;
                TextMeshProUGUI cardDescription = UtilityHelper.FindChild<TextMeshProUGUI>(cardIntactView.transform, "CardDescription");
                cardDescription.text = card.cardInfo.description;

                Image image = cardIntactView.transform.GetComponent<Image>();
                //Assets\Resources\Image\CardBG
                //image路径
                string path = "Image/Card/CardBG/";
                path = path + card.bgImageName;
                Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
                image.sprite = sprite;

                cardIntactView.card = card;
                if (isAdd) {
                    cardIntactViews.Add(cardIntactView);
                }
            }

        }
    }
}
