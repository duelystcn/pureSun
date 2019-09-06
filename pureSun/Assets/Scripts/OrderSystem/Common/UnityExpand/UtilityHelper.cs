/*************************************************************************************
     * 类 名 称：       UtilityHelper
     * 文 件 名：       UtilityHelper
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这个类是放置一些通用的unity的拓展帮助方法     
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Common.UnityExpand
{
    static class UtilityHelper
    {
        //初始化一个UI组件的rectTransform
        public static void Normalize(this RectTransform rectTransform)
        {
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }
        //找到一个object的指定名字的子对象
        public static Transform FindChild(Transform parent, string name)
        {
            Transform child = null;
            child = parent.Find(name);
            if (child != null)
                return child;
            Transform grandchild = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                grandchild = FindChild(parent.GetChild(i), name);
                if (grandchild != null)
                    return grandchild;
            }
            return null;
        }
        //找到一个object的指定类型的子对象
        public static T FindChild<T>(Transform parent, string name) where T : Component
        {
            Transform child = null;
            child = FindChild(parent, name);
            if (child != null)
                return child.GetComponent<T>();
            return null;
        }

    }
}
