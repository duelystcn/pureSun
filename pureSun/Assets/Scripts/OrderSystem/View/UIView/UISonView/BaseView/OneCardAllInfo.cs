

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.View.MinionView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class OneCardAllInfo : ViewBaseView
    {
        public CardIntactView cardIntactView;

        public CardEntry cardEntryShow { get; private set; }

        

        public EffectBuffInfoViewList effectBuffInfoViewList;

        public void LoadCardInfo(CardEntry cardEntry) {
            cardIntactView.LoadCard(cardEntry,true);

        }
        public void LoadEffectBuffInfoList(List<EffectInfo> effectBuffInfoList)
        {
            effectBuffInfoViewList.LoadEffectBuffInfoList(effectBuffInfoList);

        }

        public override void InitViewForParameter(UIControllerListMediator mediator, object body, Dictionary<string, string> parameterMap)
        {
            if (parameterMap["OtherType"] == "CardHeadView")
            {
                CardHeadView cardHeadView = body as CardHeadView;
                this.cardEntryShow = cardHeadView.card;
                this.LoadCardInfo(cardHeadView.card);
            }
            else if (parameterMap["OtherType"] == "MinionCellView")
            {
                MinionCellView minionCellView = body as MinionCellView;
                this.cardEntryShow = minionCellView.minionCellItem;
                LoadingAllInfoByMinionCellView(minionCellView);
            }
           
        }
        public void LoadingAllInfoByMinionCellView(MinionCellView minionCellView)
        {
            Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            Camera uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
            Vector3 pos = mainCamera.WorldToViewportPoint(minionCellView.transform.position);
            pos = uiCamera.ViewportToWorldPoint(pos);
            if (pos.x < 32)
            {
                pos.x += 2.7f;
            }
            else
            {
                pos.x -= 2.7f;
            }
            this.transform.position = pos;
            this.LoadingAllInfoByMinionCellItem(minionCellView.minionCellItem);
        }
        public void LoadingAllInfoByMinionCellItem(CardEntry minionCellItem)
        {
            this.LoadCardInfo(minionCellItem);
            this.LoadEffectBuffInfoList(minionCellItem.effectBuffInfoList);
        }

    }
}
