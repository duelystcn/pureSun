

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
    //UIView层级
    public class EnumUIViewLayerComparer
        : IEqualityComparer<UIViewLayer>
    {
        public bool Equals(UIViewLayer x, UIViewLayer y)
        {
            return x == y;
        }

        public int GetHashCode(UIViewLayer obj)
        {
            return (int)obj;
        }
    }
    public enum UIViewName
    {
        None,
        //开始选单
        StartMain,  
        //选择卡牌窗口
        ChooseStage,
        //复合式选择卡牌窗口
        ViewChooseMakeStage,
        //卡组列表
        CardDeckList,
        //完整的卡牌信息框
        OneCardAllInfo,
        //费用显示框
        ManaInfoView,
        //科技等级显示框
        TraitCombinationView,
        //船只框
        ShipComponentView,



        //动画组件
        //卡牌移动动画
        CardMoveAnimation,


        //


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
        TempCache,          //关闭后进入临时缓冲池
        Cache,              //关闭后常驻内存
    }

    [CreateAssetMenu(menuName = "Resources/ScriptableObject/UIViewConfig")]
    public class UIViewConfig : ScriptableObject
    {
        //是否唯一
        public bool unique = true;
        //所在层
        public UIViewLayer viewLayer;
        //界面名称
        public UIViewName viewName;
        //缓存策略  
        public UIViewCacheScheme cacheScheme;
        //资源的名称
        public string assetName;
        //被遮挡后是否需要更新
        public bool alwaysUpdate;
        //是否遮挡了整个屏幕
        public bool coverScreen;                    




    }
}
