

using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using OrderSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.EffectDisplayComponent
{
    public class EffectShowList: ViewBaseView
    {
        List<CardIntactView> cardIntactViews = new List<CardIntactView>();
        //对象池
        public ObjectPool<CardIntactView> cardIntactViewPool;

        public void InitializationEffectShowList() {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Card/CardIntactView");
            cardIntactViewPool = new ObjectPool<CardIntactView>(prefab, "handCellPool");
        }

        public void AddOneCardEffect(CardEntry cardEntry) {
            Vector3 position = new Vector3();
            CardIntactView cardIntactView = cardIntactViewPool.Pop();
            cardIntactView.transform.SetParent(transform, false);
            cardIntactView.transform.localPosition = position;
            cardIntactView.LoadCard(cardEntry,true);
            cardIntactViews.Add(cardIntactView);

        }
    }
}
