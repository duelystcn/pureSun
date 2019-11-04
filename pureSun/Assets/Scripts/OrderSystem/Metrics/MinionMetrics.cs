
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Metrics
{
    public class MinionMetrics
    {



        //返回一个长方形中心坐标
        public static Vector3 erectPosition(Vector3 position, int x,int z,string arrayMode)
        {
            if (HexMetrics.MODE_ERECT == arrayMode)
            {
                position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
                position.y = 0f;
                position.z = z * (HexMetrics.outerRadius * 1.5f);
            }
            else
            {
                position.x = x * (HexMetrics.outerRadius * 1.5f);
                position.y = 0f;
                position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

            }

            return position;
        }
        //六角坐标，因为生成第六个角时会用到第一个角，所以会有七个点
        //竖模式坐标
        public static Vector3[] erectCorners = {
        new Vector3(0f, 0f, HexMetrics.outerRadius),
        new Vector3(HexMetrics.innerRadius, 0f, 0.5f * HexMetrics.outerRadius),
        new Vector3(HexMetrics.innerRadius, 0f, -0.5f * HexMetrics.outerRadius),
        new Vector3(0f, 0f, -HexMetrics.outerRadius),
        new Vector3(-HexMetrics.innerRadius, 0f, -0.5f * HexMetrics.outerRadius),
        new Vector3(-HexMetrics.innerRadius, 0f, 0.5f * HexMetrics.outerRadius),
        new Vector3(0f, 0f, HexMetrics.outerRadius),
    };
        //六角坐标，因为生成第六个角时会用到第一个角，所以会有七个点
        //横模式
        public static Vector3[] horizCorners = {
        new Vector3(HexMetrics.outerRadius * 0.5f, 0f, HexMetrics.innerRadius),
        new Vector3(HexMetrics.outerRadius, 0f,0f),
        new Vector3(HexMetrics.outerRadius * 0.5f, 0f, -HexMetrics.innerRadius),
        new Vector3(-HexMetrics.outerRadius * 0.5f, 0f, -HexMetrics.innerRadius),
        new Vector3(-HexMetrics.outerRadius, 0f,0f),
        new Vector3(-HexMetrics.outerRadius * 0.5f, 0f, HexMetrics.innerRadius),
        new Vector3(HexMetrics.outerRadius * 0.5f, 0f, HexMetrics.innerRadius),
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
