using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Common.UnityExpand
{
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

        public static void Log(object msg, LogColor color = LogColor.BLACK)
        {
            if (color == LogColor.BLACK)
                Debug.Log(msg);
            else
                Debug.Log(string.Format(GetColor(color), msg.ToString()));
        }
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
        public static void LogError(object msg)
        {
            Debug.LogError(msg);
        }

        public static void LogWarning(object msg)
        {
            Debug.LogWarning(msg);
        }

    }
}
