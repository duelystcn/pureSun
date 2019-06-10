using Assets.Scripts.OrderSystem.Model.Hand;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    public class HandCellView : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {
        public HandCellItem handCellItem;
        // 是否处于选中状态
        private bool isDown = false;
        //被选中时调用方法
        public UnityAction OnChoose = null;

        // 当按钮被按下后系统自动调用此方法
        //选中卡牌后
        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            OnChoose();

        }
        // 当按钮抬起的时候自动调用此方法
        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
        }
    }
}
