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
    //复合界面用层级
    public enum VCSLayerSort
    {
        First,
        Second,
        Three
    }
    //单个选择界面
    public class ViewChooseStage:ViewBaseView
    {
        public CardIntactView cardIntactViewPrefab;

        //复合界面层级
        public VCSLayerSort layerSort;

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
              

                cardIntactView.LoadCard(card);
                if (isAdd) {
                    cardIntactViews.Add(cardIntactView);
                }
            }
        }
        //复合式选择框需要创建用
        public void CreateCardEntryForMake() {
            for (int n = 0; n < 5; n++) {
                CardIntactView cardIntactView = Instantiate<CardIntactView>(cardIntactViewPrefab);
                Vector3 position = new Vector3();
                cardIntactView.transform.SetParent(transform, false);
                cardIntactView.transform.localPosition = position;
                cardIntactViews.Add(cardIntactView);
            }
        }
    }
}
