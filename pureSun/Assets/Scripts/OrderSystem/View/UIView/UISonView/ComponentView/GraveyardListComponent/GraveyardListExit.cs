
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.GraveyardListComponent
{
    public class GraveyardListExit : MonoBehaviour
    {
        //被点击时调用方法
        public UnityAction OnPointerClick = () => { };

        public void PointerClick()
        {
            OnPointerClick();
        }


    }
}
