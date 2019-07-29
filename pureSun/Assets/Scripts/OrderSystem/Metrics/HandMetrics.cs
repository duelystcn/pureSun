using UnityEngine;
/**
 * 六角常量类
 */
public static class HandMetrics
{
    public const float width = 140f;

    public const float height = 170f;

    //横向间隔
    public const float interval = -10f;



    //返回一个长方形中心坐标
    public static Vector3 erectPosition(Vector3 position, int x)
    {
        position.x = (x + 0.5f) * width + interval * x - 360f;
        position.y = -300f;
        position.z = height * 0.5f;
        return position;
    }
    public static Vector3[] getCorners()
    {
        Vector3[] Corners = {
            new Vector3(width * 0.5f, 0f, height * 0.5f),
            new Vector3(width * 0.5f, 0f, -height * 0.5f),
            new Vector3(-width * 0.5f, 0f, -height * 0.5f),
            new Vector3(-width * 0.5f, 0f, height * 0.5f),
             new Vector3(width * 0.5f, 0f, height * 0.5f)
        };
        return Corners;      

    }

}