using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Common.UnityExpand
{
    static class UtilityHelper
    {
        public static void Normalize(this RectTransform rectTransform)
        {
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }

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
