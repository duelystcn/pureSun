
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class CardDeckList : ViewBaseView
    {
        public CardHeadView cardHeadPrefab;

        public ShipCardHeadView shipCardHeadViewPrefab;

        private ShipCardHeadView shipCardHeadView;

        public List<CardHeadView> cardHeadViews = new List<CardHeadView>();

        public PlayerItem playerItem;

        protected override void InitUIObjects()
        {
            base.InitUIObjects();
            if (shipCardHeadView == null)
            {
                shipCardHeadView = Instantiate<ShipCardHeadView>(shipCardHeadViewPrefab);
                Vector3 position = new Vector3();
                shipCardHeadView.transform.SetParent(transform, false);
                shipCardHeadView.transform.localPosition = position;
                shipCardHeadView.gameObject.SetActive(false);
            }



        }

        //读取玩家信息
        public void LoadPlayerInfo() {
            if (playerItem == null)
            {
                UtilityLog.LogError("playerItem is null");
            }
            if (playerItem.shipCard != null) {
                LoadShipCard(playerItem.shipCard);
            }
           
           // LoadCardList(playerItem.cardDeck);
        }
        public void LoadShipCard(CardEntry shipCard) {
            shipCardHeadView.gameObject.SetActive(true);
            shipCardHeadView.LoadCard(shipCard);

        }
        public void LoadCardList(GameContainerItem cardDeck)
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
                cardHeadView.LoadCard(card);
                if (isAdd) {
                    cardHeadViews.Add(cardHeadView);
                }
                
            }
        }
        //获取shipcard的位置
        public Vector3 GetShipCardPosition() {
            return shipCardHeadView.transform.position;

        }


    }
}
