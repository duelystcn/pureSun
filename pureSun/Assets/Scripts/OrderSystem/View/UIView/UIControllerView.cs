

using Assets.Scripts.OrderSystem.View.UIView.UISonView;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView
{
    
    public class UIControllerView : MonoBehaviour
    {
        public UIViewLayer UIViewLayer;

        //保存这一层的窗口列表,索引越大越靠近上方
        private List<ViewBaseView> views = new List<ViewBaseView>();

    }
}

