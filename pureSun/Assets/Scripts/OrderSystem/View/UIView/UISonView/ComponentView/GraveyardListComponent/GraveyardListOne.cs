
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using OrderSystem;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardListOne : MonoBehaviour
{
    public CardIntactView cardIntactViewPrefab;
    public ObjectPool<CardIntactView> cardIntactViewPool;
    public List<CardIntactView> cardIntactViewList = new List<CardIntactView>();

    public void InitView(GameContainerItem cardGraveyard)
    {
        if (cardIntactViewPool == null)
        {
            GameObject prefab = cardIntactViewPrefab.gameObject;
            cardIntactViewPool = new ObjectPool<CardIntactView>(prefab, "minionCellPool");
        }
        for (int n = 0; n < cardGraveyard.cardEntryList.Count; n++)
        {
            if (n < cardIntactViewList.Count)
            {
                cardIntactViewList[n].gameObject.SetActive(true);
                cardIntactViewList[n].LoadCard(cardGraveyard.cardEntryList[n], true);
            }
            else
            {
                CardIntactView cardIntactView = cardIntactViewPool.Pop();
                Vector3 position = new Vector3();
                cardIntactView.transform.SetParent(transform, false);
                cardIntactView.transform.localPosition = position;
                cardIntactView.LoadCard(cardGraveyard.cardEntryList[n], true);
                cardIntactViewList.Add(cardIntactView);
            }
        }
        for (int m = cardGraveyard.cardEntryList.Count; m < cardIntactViewList.Count; m++)
        {
            cardIntactViewList[m].gameObject.SetActive(false);
        }



    }
}
