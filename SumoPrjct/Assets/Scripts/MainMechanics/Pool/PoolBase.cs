using UnityEngine;
using System.Collections.Generic;
using System;

namespace ObjectsPool
{
    public class PoolBase<T> 
    {
        private Queue<T> pool = new Queue<T>();
        private List<T> active = new List<T>();

        private readonly Func<T> preloadFunc;
        private readonly Action<T> getAction;
        private readonly Action<T> returnAction;

        public PoolBase(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
        {
            this.preloadFunc = preloadFunc;
            this.getAction = getAction;
            this.returnAction = returnAction;
            if (this.preloadFunc == null)
            {
                Debug.Log("Preload func is null");
                return;
            }

            for (int i = 0; i < preloadCount; i++)
            {
                Return(preloadFunc());
            }
        }

        public T Get()
        {
            T item = pool.Count > 0 ? pool.Dequeue(): preloadFunc();
            getAction(item);
            active.Add(item);

            return item;
        }

        public void Return(T item)
        {
            returnAction(item);
            pool.Enqueue(item);
            active.Remove(item);
        }
        public void ReturnAll()
        {
            foreach (T item in active.ToArray())
            {
                Return(item);
            }
        }
        public List<T> GetAll()
        {
            List<T> all = new List<T>();
            foreach (T item in pool)
            {
                all.Add(item);
            }
            return all;
        }
    }
}
