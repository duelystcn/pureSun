

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

        //每一个界面的Order间隔
        private const int viewOrderStep = 100;
        //最上层Order值
        private int topOrder = 0;

        //压入一个新的窗口(设置为最大order)
        public void Push(ViewBaseView view)
        {
            //判断是否本来就在这个队列中
            if (view.layerController != null)
            {
                if (view.ViewOrder == topOrder)
                    return;
                else
                {
                    views.Remove(view);
                    views.Add(view);
                    topOrder += viewOrderStep;
                    view.ViewOrder = topOrder;
                }
            }
            else
            {
                views.Add(view);
                topOrder += viewOrderStep;
                PushSingleView(view);
            }
        }
        //压入单个界面
        private void PushSingleView(ViewBaseView view)
        {
            if (view != null)
            {
                view.layerController = this;
                view.OnPush();
                view.ViewOrder = topOrder;
            }
        }

    }
}

