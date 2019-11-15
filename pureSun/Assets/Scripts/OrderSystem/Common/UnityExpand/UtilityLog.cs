/*************************************************************************************
     * 类 名 称：       UtilityLog
     * 文 件 名：       UtilityLog
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这个类是拓展了Unity的log打印日志，预计在将来拓展为可以打印日志文件   
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/
using System;
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
        PURPLE
    }
    public enum LogUtType
    {
        Effect,
        Stage,
        Attack,
        Other,
        Operate,
        Special
    }

    class UtilityLog
    {
        //普通打印
        public static void Log(object msg, LogUtType logType, LogColor color = LogColor.BLACK)
        {
            if (logType == LogUtType.Effect) {
                color = LogColor.GREEN;
            }
            else if(logType == LogUtType.Attack){
                color = LogColor.PURPLE;
            }
            else if (logType == LogUtType.Operate)
            {
                color = LogColor.BLUE;
            }
            else if (logType == LogUtType.Stage)
            {
                color = LogColor.YELLOW;
            }
            else if (logType == LogUtType.Special)
            {
                color = LogColor.YELLOW;
            }


            if (color == LogColor.BLACK)
            {
                Debug.Log(msg);
            }
            else {
               // Debug.Log(string.Format(GetColor(color), msg.ToString()));
                if (logType == LogUtType.Special || logType == LogUtType.Special || logType == LogUtType.Special)
                {
                    //Debug.Log(string.Format(GetColor(color), msg.ToString() + "|||||||||" + System.Guid.NewGuid().ToString("N")));
                    Debug.Log(string.Format(GetColor(color), msg.ToString()));
                }
            }
               
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

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}
