using UnityEngine;
using System;

namespace ObjectsPool
{
    public class GameObjectPool : PoolBase<GameObject>
    {
        public GameObjectPool(
            GameObject prefab,
            int preloadCount) : base(() => Preload(prefab), GetAction, ReturnAction, preloadCount)
        {
        }

        public static GameObject Preload(GameObject prefab) => GameObject.Instantiate(prefab);
        public static void GetAction(GameObject @object) => @object.SetActive(true);
        public static void ReturnAction(GameObject @object) => @object.SetActive(false);
    }
}