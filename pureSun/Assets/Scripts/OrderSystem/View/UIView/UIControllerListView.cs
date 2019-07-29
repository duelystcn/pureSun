
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.View.UIView.UISonView;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static Assets.Scripts.OrderSystem.Common.UnityExpand.UtilitySingleton;

namespace Assets.Scripts.OrderSystem.View.UIView
{
    public class UIControllerListView : MonoBehaviour
    {
        //屏幕UI的开启列表
        private List<ViewBaseView> viewList = new List<ViewBaseView>();
        //配置文件
        private Dictionary<UIViewName, UIViewConfig> uiViewConfig = new Dictionary<UIViewName, UIViewConfig>(LiteSingleton<EnumUIViewNameComparer>.Instance);



        public Dictionary<UIViewLayer, UIControllerView> UIControllerViews = new Dictionary<UIViewLayer, UIControllerView>(LiteSingleton<EnumUIViewLayerComparer>.Instance);

        public UIControllerView controllerViewPrefab;
        //常驻内存的界面
        private List<ViewBaseView> screenUICache = new List<ViewBaseView>();
        private List<ViewBaseView> screenUITempCache = new List<ViewBaseView>();

        //临时缓冲区内的界面
        public int screenUITempCacheDepth = 0;

        //初始化
        public void AchieveUIControllerListView()
        {
            //读取配置文件
            LoadingAllUIViewConfig();
            LoadingAllUIViewLayer();
            //ShowView(UIViewName.StartMain);


        }
        //设置层级
        private void LoadingAllUIViewLayer() {
            //产生多个ui层
            foreach (int v in Enum.GetValues(typeof(UIViewLayer)))
            {
                string strName = Enum.GetName(typeof(UIViewLayer), v);
                UIControllerView UIControllerView = Instantiate<UIControllerView>(controllerViewPrefab);
                Vector3 position = new Vector3();
                UIControllerView.transform.SetParent(transform, false);
                UIControllerView.transform.localPosition = position;
                UIControllerView.name = strName;
                UIControllerView.UIViewLayer = (UIViewLayer)Enum.Parse(typeof(UIViewLayer), strName);
                UIControllerViews.Add(UIControllerView.UIViewLayer, UIControllerView);
            }

        }

        //读取配置文件
        private void LoadingAllUIViewConfig() {
            string configPath = "Assets/Resources/ScriptableObjects/UIViewConfig";
            string[] configs = Directory.GetFiles(configPath, "*.asset", SearchOption.TopDirectoryOnly);
            foreach (var item in configs)
            {
                string path = item.Replace("\\", "/");
                var config = AssetDatabase.LoadAssetAtPath<UIViewConfig>(path);
                uiViewConfig.Add(config.viewName, config);
            }
        }

        //获取名字相同的第一个界面
        public T GetViewByName<T>(UIViewName viewName)
            where T : ViewBaseView
        {
            for (int i = 0; i < viewList.Count; ++i)
            {
                if (viewList[i].config.viewName == viewName)
                {
                    return viewList[i] as T;
                }
            }
            return null;
        }

        //获取名字相同的所有界面
        public List<T> GetViewListByName<T>(UIViewName viewName)
            where T : ViewBaseView
        {
            List<T> list = new List<T>();
            for (int i = 0; i < viewList.Count; ++i)
            {
                if (viewList[i].config.viewName == viewName)
                {
                    list.Add(viewList[i] as T);
                }
            }
            return list;
        }
        //只关闭第一个同名界面
        public void HideView(UIViewName viewName)
        {
            for (int i = viewList.Count - 1; i >= 0; --i)
            {
                //关闭
                if (viewList[i].config.viewName == viewName)
                {
                    HideView(viewList[i]);
                    return;
                }
            }
        }
        //根据指定界面
        public void HideView(ViewBaseView view)
        {
            if (view == null)
                return;

            //在窗口栈中的界面都可以关闭
            if (view.layerController != null)
            {
                viewList.Remove(view);
                view.layerController.Popup(view);
                SchemeViewCache(view);
                UpdateViewHideState();
            }
            else
            {
                UtilityLog.LogError(string.Format("Attamp to hide a error view {0}, not in controller.", view.config.viewName));
            }
        }

        //根据缓存类型处理界面
        private void SchemeViewCache(ViewBaseView view)
        {
            if (view != null)
            {
                //根据缓存类型处理
                switch (view.config.cacheScheme)
                {
                    case UIViewCacheScheme.Cache:
                        CacheView(view);
                        break;
                    case UIViewCacheScheme.TempCache:
                        TempCacheView(view);
                        break;
                    case UIViewCacheScheme.AutoRemove:
                        ReleaseView(view);
                        break;
                    default:
                        break;
                }
            }
        }

        //长期缓存
        private void CacheView(ViewBaseView view)
        {
            if (!screenUICache.Contains(view))
            {
                screenUICache.Add(view);
            }
        }

        //临时缓存
        private void TempCacheView(ViewBaseView view)
        {
            //没有设置池深度，直接释放
            if (screenUITempCacheDepth <= 0)
                ReleaseView(view);

            //放入临时池中
            screenUITempCache.Add(view);

            //整理临时缓存池
            TidyTempCache();
        }

        //整理临时缓存池
        private void TidyTempCache()
        {
            int removeCount = screenUITempCache.Count - screenUITempCacheDepth;
            while (removeCount > 0)
            {
                --removeCount;
                ReleaseView(screenUITempCache[0]);
                screenUITempCache.RemoveAt(0);
            }
        }

        //释放界面
        private void ReleaseView(ViewBaseView view)
        {
            if (view != null)
            {
                view.OnExit();

#if UNITY_EDITOR
                Destroy(view.gameObject);
#endif
            }
        }

        //展示界面
        public void ShowView(UIViewName viewName, params object[] args)
        {
            //获取界面配置
            UIViewConfig config = GetConfig(viewName);
            if (config == null)
                return;
            ViewBaseView view = null;
            if (config.unique)
            {
                //判断是否打开了
                for (int i = 0; i < viewList.Count; ++i)
                {
                    if (viewList[i].config.viewName == viewName)
                    {
                        view = viewList[i];
                        break;
                    }
                }
                //判断是否已被打开，没有打开个新的
                if (view != null)
                {
                    if (view.layerController == null)
                    {
                        UtilityLog.LogError(string.Format("Show view error: {0}, not layer", viewName));
                        return;
                    }
                    //设置参数，重新放入窗口层级控制器
                   // view.SetArguments(args);
                    view.layerController.Push(view);
                }
                else
                {
                   ShowViewFromCacheOrCreateNew(config, args);
                }
            }
            else
            {
                ShowViewFromCacheOrCreateNew(config, args);
            }
            //刷新显示、隐藏状态
            UpdateViewHideState();

        }
        //先尝试从缓存中打开，如果失败则打开一个新的
        private void ShowViewFromCacheOrCreateNew(UIViewConfig config, params object[] args)
        {
            //先尝试从缓存区中读取
            ViewBaseView view = GetViewFromCache(config);

            //缓存区内没有，打开新的
            if (view == null) {
                view = ShowNewView(config);
            }
            if (view != null) {
                PushViewToLayer(view, args);
            }
            else {
                UtilityLog.LogError(string.Format("Show view failed -> {0}", config.viewName));
            }   
        }
        //放入层级中
        private void PushViewToLayer(ViewBaseView view, params object[] args)
        {
            if (view != null)
            {
                //设置参数
               // view.SetArguments(args);
                //添加到相应的列表
                viewList.Add(view);
                //压入对应的层中
                UIControllerViews[view.config.viewLayer].Push(view);
            }
        }
        //打开一个新的
        private ViewBaseView ShowNewView(UIViewConfig config)
        {
            if (!UIControllerViews.ContainsKey(config.viewLayer))
            {
                UtilityLog.LogError("Show new view failed. Layer error.");
                return null;
            }
            //加载
            ViewBaseView view = CreateUIView(config);
            if (view)
            {
                //创建完毕，初始化
                view.Init();
                view.transform.SetParent(UIControllerViews[config.viewLayer].transform, false);
                view.GetComponent<RectTransform>().Normalize();
            }
            return view;
        }
        //读入界面
        private ViewBaseView CreateUIView(UIViewConfig config)
        {
            GameObject obj = null;
            obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/Resources/Prefabs/UIView/{0}.prefab", config.assetName));
            if (obj != null)
                obj = Instantiate(obj);

            if (obj)
            {
                var viewBase = obj.GetComponent<ViewBaseView>();
                return viewBase;
            }
            else {
                UtilityLog.LogError(string.Format("Load view error: no view : {0}", config.assetName));
                return null;
            }
        }


        //从池中获取界面
        private ViewBaseView GetViewFromCache(UIViewConfig config)
        {
            if (config == null)
                return null;

            ViewBaseView view = null;
            List<ViewBaseView> cache = null;

            switch (config.cacheScheme)
            {
                case UIViewCacheScheme.Cache:
                    cache = screenUICache;
                    break;
                case UIViewCacheScheme.TempCache:
                    cache = screenUITempCache;
                    break;
                default:
                    break;
            }
            if (cache != null)
            {
                for (int i = 0; i < cache.Count; ++i)
                {
                    if (cache[i].config.viewName == config.viewName)
                    {
                        view = cache[i];
                        //从缓冲区中移除
                        cache.RemoveAt(i);
                        break;
                    }
                }
            }
            return view;
        }
        //根据名称获取配置
        private UIViewConfig GetConfig(UIViewName viewName)
        {
            if (uiViewConfig.ContainsKey(viewName) == false)
            {
                UtilityLog.LogError(string.Format("Get view config error: {0}", viewName));
                return null;
            }
            return uiViewConfig[viewName];
        }
        //刷新界面的隐藏情况
        private void UpdateViewHideState()
        {
            //从最上层开始刷新
            bool covered = false;
            covered = UIControllerViews[UIViewLayer.Debug].RefreshView(covered);
            covered = UIControllerViews[UIViewLayer.Top].RefreshView(covered);
            covered = UIControllerViews[UIViewLayer.Popup].RefreshView(covered);
            covered = UIControllerViews[UIViewLayer.Base].RefreshView(covered);
            covered = UIControllerViews[UIViewLayer.Background].RefreshView(covered);
        }


    }
}
