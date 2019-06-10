
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Metrics
{
    public class MinionMetrics
    {

        //边长
        public const float outerRadius = 10f;
        //边到中心的长度
        public const float innerRadius = outerRadius * 0.866025404f;

        //返回一个长方形中心坐标
        public static Vector3 erectPosition(Vector3 position, int x,int z,string arrayMode)
        {
            if (HexMetrics.MODE_ERECT == arrayMode)
            {
                position.x = (x + z * 0.5f - z / 2) * (innerRadius * 2f);
                position.y = 0f;
                position.z = z * (outerRadius * 1.5f);
            }
            else
            {
                position.x = x * (outerRadius * 1.5f);
                position.y = 0f;
                position.z = (z + x * 0.5f - x / 2) * (innerRadius * 2f);

            }

            return position;
        }
        //六角坐标，因为生成第六个角时会用到第一个角，所以会有七个点
        //竖模式坐标
        public static Vector3[] erectCorners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius),
    };
        //六角坐标，因为生成第六个角时会用到第一个角，所以会有七个点
        //横模式
        public static Vector3[] horizCorners = {
        new Vector3(outerRadius * 0.5f, 0f, innerRadius),
        new Vector3(outerRadius, 0f,0f),
        new Vector3(outerRadius * 0.5f, 0f, -innerRadius),
        new Vector3(-outerRadius * 0.5f, 0f, -innerRadius),
        new Vector3(-outerRadius, 0f,0f),
        new Vector3(-outerRadius * 0.5f, 0f, innerRadius),
        new Vector3(outerRadius * 0.5f, 0f, innerRadius),
    };
        public static Vector3[] getCornersByArrayMode(string arrayMode)
        {
            if (HexMetrics.MODE_ERECT == arrayMode)
            {
                return erectCorners;
            }
            else
            {
                return horizCorners;
            }

        }
    }
}
