using Assets.Scripts.OrderSystem.Model.Hex;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View
{
    public class HexCellView: MonoBehaviour
    {
        public HexCellItem hexCellItem;
        public UnityAction OnClick = null;
    }

}
