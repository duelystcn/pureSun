using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.View.HexView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View
{
    public class HexCellView: MonoBehaviour
    {
        public HexCellItem hexCellItem;
        public UnityAction OnClick = null;
        public MonoBehaviour hexGroundComponent;

       
        public void LoadHexCellItem(HexCellItem hexCellItem, HexGridView hexGridView)
        {
            this.hexCellItem = hexCellItem;
            UtilityImage image = hexGroundComponent.transform.GetComponent<UtilityImage>();
            if (hexCellItem.borderState == BorderState.CanCall)
            {
                image.material = hexGridView.canCallBorderColor;
            }
            else if (hexCellItem.borderState == BorderState.Normal)
            {
                image.material = hexGridView.originalBorderColor;
            }


        }
    }

}
