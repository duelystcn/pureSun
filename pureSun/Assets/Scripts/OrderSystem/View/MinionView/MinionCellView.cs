using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Minion;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionCellView : MonoBehaviour
    {
        //被选中时调用方法
        public UnityAction OnPointerEnter = () => { };
        public UnityAction OnPointerExit = () => { };
        public MinionCellItem minionCellItem;

        public void PointerEnter()
        {
            OnPointerEnter();
          
        }


        public void PointerExit()
        {
            OnPointerExit();
          
        }
    }
}
