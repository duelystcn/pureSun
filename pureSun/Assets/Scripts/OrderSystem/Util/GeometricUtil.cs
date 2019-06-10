

using System;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Util
{
    public class GeometricUtil
    {   
        //判断一个点是否在矩形内
        public static bool CheckPointInRectangle(float width,float height, Vector3 position) {
            int x = Math.Abs(Mathf.RoundToInt(position.x / (width / 2f)));
            int z = Math.Abs(Mathf.RoundToInt(position.z / (height / 2f)));
            if (x >= 1 || z >= 1)
            {
                return false;
            }
            else {
                return true;
            }
        }
        //判断一个点是否在指定区域内
        public static bool CheckPointInSomeRectangle(float width, float height, Vector3 startPosition, Vector3 checkPosition)
        {
            int x = Math.Abs(Mathf.RoundToInt((checkPosition.x - startPosition.x) / (width / 2f)));
            int z = Math.Abs(Mathf.RoundToInt((checkPosition.z - startPosition.z) / (height / 2f)));
            if (x >= 1 || z >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
