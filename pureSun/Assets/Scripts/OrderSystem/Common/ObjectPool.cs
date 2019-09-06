
/*************************************************************************************
     * 类 名 称：       ObjectPool
     * 文 件 名：       ObjectPool
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        这是一个泛型对象池，可以方便重复利用object，而不用创建新的
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using System;

namespace OrderSystem
{
    //对象池
    public class ObjectPool<T> where T :MonoBehaviour
    {
        private string poolName = null;
        public string PoolName
        {
            get { return poolName; }
        }
        private GameObject prefab = null;
        private IList<GameObject> pool = null;
        public ObjectPool( GameObject prefab  , string name = "Pool" )
        {
            this.prefab = prefab;
            poolName = name;
            pool = new List<GameObject>();
        }

        public IList<T> Pop( int count )
        {
            IList<T> result = new List<T>();
            for ( int i = 0 ; i < count ; i++ )
                result.Add(Pop());
            return result;
        }
        public T Pop()
        {
            if (pool.Count > 0)
            {
                var result = pool[0];
                result.SetActive(true);
                pool.RemoveAt(0);
                return result.GetComponent<T>();
            }
            return Create();
        }
        public void Push( GameObject go )
        {
            go.SetActive(false);
            pool.Add(go);
        }
        public void Push( T t)
        {
            Push(t.gameObject);
        }
        private T Create()
        {
            var obj = UnityEngine.Object.Instantiate(prefab);
            return obj.GetComponent<T>();
        }
    }
}