
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView
{
    public class ViewBaseView : MonoBehaviour
    {
        public UIViewConfig config;

        [HideInInspector]
        public UIControllerView layerController;   //所属的层
    }
}
