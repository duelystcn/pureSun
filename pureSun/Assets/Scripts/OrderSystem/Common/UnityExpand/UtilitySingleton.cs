

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
