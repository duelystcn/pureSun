
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



        public Dictionary<UIViewLayer, UIControllerView> UIControllerViews;
        public UIControllerView controllerViewPrefab;
        //初始化
        public void AchieveUIControllerListView()
        {
            //读取配置文件
            LoadingAllUIViewConfig();
            LoadingAllUIViewLayer();


        }
        //设置层级
        private void LoadingAllUIViewLayer() {
            //产生多个ui层
            UIControllerViews = new Dictionary<UIViewLayer, UIControllerView>();
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
            string configPath = "Assets/HalfSLG/ScriptableObjects/UIView";
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

        //展示界面
        public void ShowView(UIViewName viewName, params object[] args)
        {
            //获取界面配置
            UIViewConfig config = GetConfig(viewName);
            if (config == null)
                return;

           
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


    }
}
