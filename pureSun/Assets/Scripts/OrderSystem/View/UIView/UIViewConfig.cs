

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView
{
    //添加后需要在初始化时注册
    public enum UIViewLayer
    {
        Background,
        Base,
        Popup,
        Top,
        Debug,
    }
    //添加后需要在初始化时注册
    public enum UIViewName
    {
        None,
        //开始选单
        StartMain,                       
    }
    //UIView名字的比较器
    public class EnumUIViewNameComparer
        : IEqualityComparer<UIViewName>
    {
        public bool Equals(UIViewName x, UIViewName y)
        {
            return x == y;
        }

        public int GetHashCode(UIViewName obj)
        {
            return (int)obj;
        }
    }
    public enum UIViewCacheScheme
    {
        AutoRemove,         //自动移除
        TempCache,           //关闭后进入临时缓冲池
        Cache,              //关闭后常驻内存
    }

    [CreateAssetMenu(menuName = "Resources/ScriptableObject/UIViewConfig")]
    public class UIViewConfig : ScriptableObject
    {
        //所在层
        public UIViewLayer viewLayer;
        //界面名称
        public UIViewName viewName;
        //缓存策略  
        public UIViewCacheScheme cacheScheme;       




    }
}
