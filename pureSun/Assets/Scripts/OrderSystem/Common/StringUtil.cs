
/*************************************************************************************
     * 类 名 称：       RandomUtil
     * 文 件 名：       RandomUtil
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        用于处理字符串
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/


using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assets.Scripts.OrderSystem.Common
{
    public class StringUtil
    {

        //将notificationType和playerCode拼接在一起
        public static string NotificationTypeAddPlayerCode(string type, string playerCode) {
            string over = type + "&" + playerCode;
            return over;
        }
        //从拼接的字符串中获取notificationType
        public static string GetNotificationTypeForNP(string str) {
            string[] sArray = Regex.Split(str, "&", RegexOptions.IgnoreCase);
            return sArray[0];
        }
        //从拼接的字符串中获取playerCode
        public static string GetPlayerCodeForNP(string str)
        {
            string[] sArray = Regex.Split(str, "&", RegexOptions.IgnoreCase);
            if (sArray.Length > 1)
            {
                return sArray[1];
            }
            else {
                return "";
            }
        }
        //从拼接的字符串中获取转发的NotificationName
        public static string GeNotificationNameForNN(string str)
        {
            string[] sArray = Regex.Split(str, "=>", RegexOptions.IgnoreCase);
            if (sArray.Length > 1)
            {
                return sArray[1];
            }
            else
            {
                UtilityLog.LogError("转发失败");
                return "";
            }
        }
        //传入一个notificationType返回一个map的json
        public static string GetNTByNotificationType(string notificationType) {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,DelayedProcess返回一个map的json
        public static string GetNTByNotificationTypeAndDelayedProcess(string notificationType, string delayedProcess)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("DelayedProcess", delayedProcess);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,playerCode返回一个map的json
        public static string GetNTByNotificationTypeAndPlayerCode(string notificationType,string playerCode)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("PlayerCode", playerCode);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,UIViewName返回一个map的json
        public static string GetNTByNotificationTypeAndUIViewName(string notificationType, string UIViewName)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("UIViewName", UIViewName);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,UIViewName,MaskLayer返回一个map的json
        public static string GetNTByNotificationTypeAndUIViewNameAndMaskLayer(string notificationType, string UIViewName, string MaskLayer)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("UIViewName", UIViewName);
            TestMap.Add("OpenMaskLayer", MaskLayer);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,UIViewName,PlayerCode,MaskLayer返回一个map的json
        public static string GetNTByNotificationTypeAndUIViewNameAndMaskLayerAndPlayerCode(string notificationType, string UIViewName, string playerCode, string MaskLayer)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("UIViewName", UIViewName);
            TestMap.Add("OpenMaskLayer", MaskLayer);
            TestMap.Add("PlayerCode", playerCode);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,UIViewName,OtherType返回一个map的json
        public static string GetNTByNotificationTypeAndUIViewNameAndOtherType(string notificationType, string UIViewName, string otherType)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("UIViewName", UIViewName);
            TestMap.Add("OtherType", otherType);
            return JsonConvert.SerializeObject(TestMap);
        }
        //传入一个notificationType,UIViewName,OtherType,DelayedProcess返回一个map的json
        public static string GetNTByNotificationTypeAndUIViewNameAndOtherTypeAndDelayedProcess(string notificationType, string UIViewName, string otherType, string delayedProcess)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("UIViewName", UIViewName);
            TestMap.Add("OtherType", otherType);
            TestMap.Add("DelayedProcess", delayedProcess);
            return JsonConvert.SerializeObject(TestMap);
        }

        //传入一个notificationType,playerCode,UIViewName返回一个map的json
        public static string GetNTByNotificationTypeAndPlayerCodeAndUIViewName(string notificationType, string playerCode, string UIViewName)
        {
            Dictionary<string, string> TestMap = new Dictionary<string, string>();
            TestMap.Add("NotificationType", notificationType);
            TestMap.Add("UIViewName", UIViewName);
            TestMap.Add("PlayerCode", playerCode);
            return JsonConvert.SerializeObject(TestMap);
        }

        //传入一个key值获取map-json中对应的值
        public static string GetValueForNotificationTypeByKey(string notificationTypeJson,string key) {
            if (notificationTypeJson.Contains(key))
            {
                Dictionary<string, string> notificationTypeMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(notificationTypeJson);
                return notificationTypeMap[key];
            }
            else {
                if (key == "NotificationType") {
                    return notificationTypeJson;
                }
                return null;
            }
        }
        //直接返回map
        public static Dictionary<string, string> GetParameterMapForNotificationType(string notificationTypeJson)
        {
            Dictionary<string, string> notificationTypeMap = new Dictionary<string, string>();
            if (notificationTypeJson.Contains("NotificationType"))
            {
                notificationTypeMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(notificationTypeJson);
                if (!notificationTypeJson.Contains("PlayerCode"))
                {
                    notificationTypeMap.Add("PlayerCode", "");
                }
                return notificationTypeMap;
            }
            else
            {
                notificationTypeMap.Add("NotificationType", notificationTypeJson);
                notificationTypeMap.Add("PlayerCode", "");
                return notificationTypeMap;
            }
        }

    }
}
