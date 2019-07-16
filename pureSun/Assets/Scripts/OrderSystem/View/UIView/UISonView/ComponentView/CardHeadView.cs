

using Assets.Scripts.OrderSystem.Model.Database.Card;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView
{
    public class CardHeadView : MonoBehaviour
    {
        public CardEntry card;
        public UnityAction OnClick;
        public void PointerClick() {
            OnClick();
        }
    }
}
