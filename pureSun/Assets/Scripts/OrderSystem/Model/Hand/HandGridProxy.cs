using Assets.Scripts.OrderSystem.Event;
using OrderSystem;
using PureMVC.Patterns.Proxy;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Hand
{
    public class HandGridProxy : Proxy
    {
        public new const string NAME = "HandGridProxy";
        public HandGridItem handGridItem
        {
            get { return (HandGridItem)base.Data; }
        }
        public HandGridProxy() : base(NAME)
        {
            HandGridItem handGridItem = new HandGridItem();
            handGridItem.Create();
            base.Data = handGridItem;
        }
        public void NowPlayerHandAfflux(HandGridItem handGridItem) {
            base.Data = handGridItem;
            SendNotification(HandSystemEvent.HAND_CHANGE, handGridItem, HandSystemEvent.HAND_CHANGE_AFFLUX);
        }
        //移除一张手牌的实例
        public void RemoveOneHandCellItem(HandCellItem handCellItem) {
            int index = -1;
            for (int i = 0; i < handGridItem.handCells.Count; i++) {
                if (handGridItem.handCells[i].X == handCellItem.X) {
                    index = i;
                }
            }
            if (index < 0)
            {
                Debug.LogError("This handCellItem index" + handCellItem.X + "is not exist");
            }
            else {
                handGridItem.handCells.RemoveAt(index);
            }
        }
    }
}
