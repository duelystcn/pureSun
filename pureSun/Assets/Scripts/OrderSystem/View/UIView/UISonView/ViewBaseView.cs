
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView
{
    public enum UIViewState
    {
        Nonvisible, //不可见的
        Visible,    //可见的
        Cache,      //在缓存中
    }
    public class ViewBaseView : MonoBehaviour
    {
        public UIViewConfig config;
        //所属的层
        [HideInInspector]
        public UIControllerView layerController;

        protected Canvas canvas;
        //当前界面的状态
        private UIViewState viewState;                  
        public UIViewState ViewState
        {
            get
            {
                return viewState;
            }
            set
            {
                viewState = value;
                switch (viewState)
                {
                    case UIViewState.Nonvisible:
                        name = string.Format("{0}(HIDE)", config.assetName);
                        break;
                    case UIViewState.Visible:
                        name = config.assetName;
                        break;
                    case UIViewState.Cache:
                        if (config.cacheScheme == UIViewCacheScheme.Cache)
                            name = string.Format("{0}(CACHE)", config.assetName);
                        else if (config.cacheScheme == UIViewCacheScheme.TempCache)
                            name = string.Format("{0}(TEMP)", config.assetName);
                        break;
                    default:
                        break;
                }
            }
        }

        //设置界面层级
        public int ViewOrder
        {
            get
            {
                return canvas.sortingOrder;
            }
            set
            {
                canvas.sortingOrder = value;
            }
        }
        //界面初始化
        public void Init()
        {
            InitCanvas();
            InitUIObjects();
            //InitBG();
        }
        //初始化各UI对象
        protected virtual void InitUIObjects() { }
        private void InitCanvas()
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
            }

            //添加射线检测
            GraphicRaycaster caster = GetComponent<GraphicRaycaster>();
            if (caster == null)
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }
        }

        //被压入到窗口栈中
        public virtual void OnPush()
        {
            ViewState = UIViewState.Nonvisible;

            if (!canvas.overrideSorting)
                canvas.overrideSorting = true;

            switch (config.viewLayer)
            {
                case UIViewLayer.Background:
                    canvas.sortingLayerID = UIVIewMetrics.SortingLayer_UI_Background;
                    break;
                case UIViewLayer.Base:
                    canvas.sortingLayerID = UIVIewMetrics.SortingLayer_UI_Base;
                    break;
                case UIViewLayer.Popup:
                    canvas.sortingLayerID = UIVIewMetrics.SortingLayer_UI_Popup;
                    break;
                case UIViewLayer.Top:
                    canvas.sortingLayerID = UIVIewMetrics.SortingLayer_UI_Top;
                    break;
                case UIViewLayer.Debug:
                    canvas.sortingLayerID = UIVIewMetrics.SortingLayer_UI_Debug;
                    break;
                default:
                    UtilityLog.LogError(string.Format("Set Layer And Order failed: Error layer -> {0}", config.viewLayer));
                    break;
            }
        }

    }
}
