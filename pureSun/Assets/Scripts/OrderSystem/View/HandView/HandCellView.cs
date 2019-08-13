using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    public class HandCellView : ViewBaseView
    {
       
        // 是否处于选中状态
        private bool isDown = false;
       


        public HandCellItem handCellItem;
        //视图
        public CardIntactView cardIntactView;
        public HandCellInstance handCellInstance;





        //被选中时调用方法
        public UnityAction OnPointerDown = () => { };
        public UnityAction OnPointerEnter = () => { };
        public UnityAction OnPointerExit = () => { };

        public void LoadHandCellItem(HandCellItem handCellItem)
        {
            this.handCellItem = handCellItem;
            //详细图读取后隐藏
            cardIntactView.LoadCard(handCellItem.cardEntry);
            cardIntactView.gameObject.SetActive(false);
            //缩略图读取
            handCellInstance.LoadCard(handCellItem.cardEntry);

            SetCanUseOutLight(handCellItem);
        }

        public void SetCanUseOutLight(HandCellItem handCellItem) {
            handCellInstance.SetOutLight(handCellItem.canUse);
            cardIntactView.SetOutLight(handCellItem.canUse);
        }

        public void PointerEnter()
        {
            OnPointerEnter();
            LayoutElement layoutElement = this.GetComponent<LayoutElement>();
            layoutElement.minWidth = 250;
            this.cardIntactView.gameObject.SetActive(true);
        }


        public void PointerExit()
        {
            if (!isDown) {
                OnPointerExit();
                LayoutElement layoutElement = this.GetComponent<LayoutElement>();
                layoutElement.minWidth = 179;
                this.cardIntactView.gameObject.SetActive(false);
            }
        }


        // 当按钮被按下后系统自动调用此方法
        //选中卡牌后
        public void PointerDown()
        {
            isDown = true;
            OnPointerDown();



        }
        // 当按钮抬起的时候自动调用此方法
        public void PointerUp()
        {
            isDown = false;
           
        }
    }
}
