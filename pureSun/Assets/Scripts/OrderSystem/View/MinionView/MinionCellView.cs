using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Minion;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.View.MinionView.MinionGridMediator;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionCellView : MonoBehaviour
    {
        public string playerCode;

        //被选中时调用方法
        public UnityAction OnPointerEnter = () => { };
        public UnityAction OnPointerExit = () => { };
        public SendNotificationConfirmTargetMinion OnPointerClick = (CardEntry minionCellItem) => { };
        public SendNotificationConfirmTargetMinion OnPointerDown = (CardEntry minionCellItem) => { };
        public SendNotificationConfirmTargetMinion OnPointerUp = (CardEntry minionCellItem) => { };
        public CardEntry minionCellItem;

        public void PointerEnter()
        {
            OnPointerEnter();
          
        }


        public void PointerExit()
        {
            OnPointerExit();
          
        }

        public void PointerClick()
        {
            OnPointerClick(this.minionCellItem);

        }
        public void PointerDown()
        {
            OnPointerDown(this.minionCellItem);

        }
        public void PointerUp()
        {
            OnPointerUp(this.minionCellItem);

        }



    }
}
