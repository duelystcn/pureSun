
/*************************************************************************************
     * 类 名 称：       CollectionUtil
     * 文 件 名：       CollectionUtil
     * 创建时间：       2019-09-10
     * 作    者：       chenxi
     * 说   明：        这里是关于集合的公共方法
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/


using System;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Common
{
    public class CollectionUtil
    {
        /// <summary>
        /// 将枚举转为集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> EnumToList<T>()
        {
            List<T> list = new List<T>();
            Array arrays = Enum.GetValues(typeof(T));
            for (int i = 0; i < arrays.LongLength; i++)
            {
                T tmp = (T)arrays.GetValue(i);
                list.Add(tmp);
            }

            return list;
        }
    }

  
}
