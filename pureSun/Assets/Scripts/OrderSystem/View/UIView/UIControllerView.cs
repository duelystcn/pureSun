

using Assets.Scripts.OrderSystem.Common.UnityExpand;
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
        //弹出一个指定窗口
        public void Popup(ViewBaseView view)
        {
            if (view == null)
                return;

            bool err = true;
            for (int i = views.Count - 1; i >= 0; --i)
            {
                if (views[i].GetInstanceID() == view.GetInstanceID())
                {
                    views.RemoveAt(i);
                    PopupSingleView(view);
                    err = false;
                    break;
                }
            }

            if (err)
            {
                UtilityLog.LogError(string.Format("Popup view failed. Can not find {0} in {1}", view.config.viewName, UIViewLayer));
                return;
            }

            //刷新最大order
            RefreshTopOrder();
        }
        //弹出单个界面
        private void PopupSingleView(ViewBaseView view)
        {
            if (view != null)
            {
                view.ViewOrder = 0;
                view.layerController = null;
                view.OnPopup();
            }
        }
        private void RefreshTopOrder()
        {
            if (views.Count == 0)
                topOrder = 0;
            else
                topOrder = views[views.Count - 1].ViewOrder;
        }

        //刷新界面的显示
        public bool RefreshView(bool alreadyCovered)
        {
            //如果已经覆盖了全部的屏幕
            if (alreadyCovered)
            {
                for (int i = views.Count - 1; i >= 0; --i)
                {
                    if (views[i].config.alwaysUpdate)
                        views[i].OnShow();
                    else
                        views[i].OnHide();
                }
                return true;
            }
            //当前还没有覆盖整个屏幕
            else
            {
                bool covered = false;
                for (int i = views.Count - 1; i >= 0; --i)
                {
                    if (views[i].config.alwaysUpdate)
                        views[i].OnShow();
                    else
                    {
                        if (covered)
                            views[i].OnHide();
                        else
                            views[i].OnShow();
                    }

                    if (!covered)
                        covered = views[i].config.coverScreen;
                }
                return covered;
            }
        }

    }
}

