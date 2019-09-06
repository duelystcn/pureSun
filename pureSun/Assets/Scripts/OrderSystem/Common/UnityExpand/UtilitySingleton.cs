/*************************************************************************************
     * 类 名 称：       UtilitySingleton
     * 文 件 名：       UtilitySingleton
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这个类是用于泛型，目前项目中还没有用到的地方  
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

namespace Assets.Scripts.OrderSystem.Common.UnityExpand
{
    class UtilitySingleton
    {
        //简单泛型
        public class LiteSingleton<T>
            where T : new()
        {
            private static T instance;
            public static T Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }

                    return instance;
                }
            }
        }

    }
}
