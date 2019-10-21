
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
        //是否需要刷新
        private bool dirty = false;                    
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
        //需要通过传递参数和引用消息系统的初始化
        public virtual void InitViewForParameter(UIControllerListMediator mediator, object body) { }
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
        //改变贴图
        public void changeImageSprite(MonoBehaviour monoBehaviour, string path)
        {
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            Image image = monoBehaviour.transform.GetComponent<Image>();
            image.sprite = sprite;
        }
        //被移出窗口栈
        public virtual void OnPopup()
        {
            if (ViewState == UIViewState.Cache)
                return;

            //如果不是隐藏状态，需要先隐藏
            if (ViewState == UIViewState.Visible)
                OnHide();

            //UtilityLog.Log(string.Format("View On Popup : {0}, {1}", config.viewName, this.GetInstanceID()));
            ViewState = UIViewState.Cache;

        }
        //被隐藏
        public virtual void OnHide()
        {
            if (ViewState == UIViewState.Visible)
            {
                //从相机的视域体内推出
                Vector3 pos = transform.localPosition;
                pos.z = -9999;
                transform.localPosition = pos;

                ViewState = UIViewState.Nonvisible;

                //UtilityLog.Log(string.Format("View On Hide : {0}, {1}", config.viewName, this.GetInstanceID()));
            }
        }

        //将被移除
        public virtual void OnExit()
        {
            //如果不是缓存池状态，则需要先弹出
            if (ViewState != UIViewState.Cache)
                OnPopup();

            //UtilityLog.Log(string.Format("View On Exit : {0}, {1}", config.viewName, this.GetInstanceID()));
        }

        //被显示时
        public virtual void OnShow()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (ViewState != UIViewState.Visible)
            {
                //将z坐标归0
                Vector3 pos = transform.localPosition;
                pos.z = 0;
                transform.localPosition = pos;

                ViewState = UIViewState.Visible;

                //UtilityLog.Log(string.Format("View On Show : {0}, {1}", config.viewName, this.GetInstanceID()));
            }

            if (dirty)
                UpdateView();
        }
        //更新
        public virtual void UpdateView()
        {
            dirty = false;
            UtilityLog.Log(string.Format("Update View -> {0}, {1}", config.viewName, this.GetInstanceID()));
        }


    }
}
