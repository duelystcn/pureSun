
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class CardDeckList : ViewBaseView
    {
        public CardHeadView cardHeadPrefab;

        public List<CardHeadView> cardHeadViews = new List<CardHeadView>();

        public PlayerItem playerItem;

        //读取玩家信息
        public void LoadPlayerInfo() {
            if (playerItem == null)
            {
                UtilityLog.LogError("playerItem is null");
            }
            LoadShipCard(playerItem.shipCard);
            LoadCardList(playerItem.cardDeck);
        }
        public void LoadShipCard(CardEntry shipCard) {
            
           
        }
        public void LoadCardList(CardDeck cardDeck)
        {
            for (int i = 0; i < cardDeck.cardEntryList.Count; i++)
            {
                CardEntry card = cardDeck.cardEntryList[i];
                CardHeadView cardHeadView = null;
                bool isAdd = true;
                if (i < cardHeadViews.Count)
                {
                    cardHeadView = cardHeadViews[i];
                    isAdd = false;
                }
                else
                {
                    cardHeadView = Instantiate<CardHeadView>(cardHeadPrefab);
                    Vector3 position = new Vector3();
                    cardHeadView.transform.SetParent(transform, false);
                    cardHeadView.transform.localPosition = position;

                }
                TextMeshProUGUI cardName = UtilityHelper.FindChild<TextMeshProUGUI>(cardHeadView.transform, "CardName");
                cardName.text = card.cardInfo.name;

                string path = "Image/Card/CardHead/";
                path = path + card.bgImageName + "_head";
                Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
                Image image = cardHeadView.transform.GetComponent<Image>();
                image.sprite = sprite;

                cardHeadView.card = card;
                if (isAdd) {
                    cardHeadViews.Add(cardHeadView);
                }
                
            }
        }
    }
}
