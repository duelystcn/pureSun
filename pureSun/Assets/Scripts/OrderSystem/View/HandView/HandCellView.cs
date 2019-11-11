using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    public class HandCellView : ViewBaseView
    {
       
        // 是否处于选中状态
        private bool isDown = false;
       


        public CardEntry handCellItem;
        //视图
        public CardIntactView cardIntactView;
        public HandCellInstance handCellInstance;





        //被选中时调用方法
        public UnityAction OnPointerDown = () => { };
        public UnityAction OnPointerEnter = () => { };
        public UnityAction OnPointerExit = () => { };

        public bool myself;
        public void LoadHandCellItem(CardEntry handCellItem)
        {
            this.handCellItem = handCellItem;
            //详细图读取后隐藏
            cardIntactView.LoadCard(handCellItem, myself);
            cardIntactView.gameObject.SetActive(false);
            //缩略图读取
            handCellInstance.LoadCard(handCellItem, myself);
            if (myself) {
                SetCanUseOutLight(handCellItem);
            }
        }
        //设置可用给高亮
        public void SetCanUseOutLight(CardEntry handCellItem) {
            this.handCellItem.canUse = handCellItem.canUse;
            handCellInstance.SetOutLight(handCellItem.canUse);
            cardIntactView.SetOutLight(handCellItem.canUse);
        }

        //取消选中状态
        public void UncheckChange()
        {
            isDown = false;
            PointerExit();
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
            if (!isDown)
            {
                OnPointerExit();
                LayoutElement layoutElement = this.GetComponent<LayoutElement>();
                layoutElement.minWidth = 179;
                this.cardIntactView.gameObject.SetActive(false);
            }
            else {
                if (!handCellItem.canUse) {
                    OnPointerExit();
                    LayoutElement layoutElement = this.GetComponent<LayoutElement>();
                    layoutElement.minWidth = 179;
                    this.cardIntactView.gameObject.SetActive(false);
                }
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
