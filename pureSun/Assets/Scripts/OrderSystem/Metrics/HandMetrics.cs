using UnityEngine;
/**
 * 六角常量类
 */
public static class HandMetrics
{
    public const float width = 16f;

    public const float height = 20f;
    //横向间隔
    public const float interval = 4f;



    //返回一个长方形中心坐标
    public static Vector3 erectPosition(Vector3 position, int x)
    {
        position.x = (x + 0.5f) * width + interval * x;
        position.y = 0f;
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