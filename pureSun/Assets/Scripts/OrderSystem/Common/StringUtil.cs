
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

    }
}
