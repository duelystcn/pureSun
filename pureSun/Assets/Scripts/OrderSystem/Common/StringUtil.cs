

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
    }
}
