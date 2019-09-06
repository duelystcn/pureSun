﻿/*************************************************************************************
     * 类 名 称：       UtilityLog
     * 文 件 名：       UtilityLog
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这个类是拓展了Unity的log打印日志，预计在将来拓展为可以打印日志文件   
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Common.UnityExpand
{
    //颜色
    public enum LogColor
    {
        BLACK,
        RED,
        GREEN,
        BLUE,
        YELLOW,
        PURPLE,
    }
    class UtilityLog
    {
        //普通打印
        public static void Log(object msg, LogColor color = LogColor.BLACK)
        {
            if (color == LogColor.BLACK)
                Debug.Log(msg);
            else
                Debug.Log(string.Format(GetColor(color), msg.ToString()));
        }
        //获取颜色
        private static string GetColor(LogColor color)
        {
            switch (color)
            {
                case LogColor.BLACK:
                    return "<color=#000000>{0}</color>";
                case LogColor.RED:
                    return "<color=#FF0000>{0}</color>";
                case LogColor.GREEN:
                    return "<color=#00FF00>{0}</color>";
                case LogColor.BLUE:
                    return "<color=#0000FF>{0}</color>";
                case LogColor.YELLOW:
                    return "<color=#FFFF00>{0}</color>";
                case LogColor.PURPLE:
                    return "<color=#FF00FF>{0}</color>";
                default:
                    return "<color=#000000>{0}</color>";
            }
        }
        //打印错误
        public static void LogError(object msg)
        {
            Debug.LogError(msg);
        }
        //打印警告
        public static void LogWarning(object msg)
        {
            Debug.LogWarning(msg);
        }

    }
}
