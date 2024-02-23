using ObjectsPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPool : MonoBehaviour
{
    const int HEAL_PRELOAD_COUNT = 7;

    [SerializeField] private GameObject healPrefab;
    private GameObjectPool healObjectsPool;
    private void Awake()
    {
        healObjectsPool = new GameObjectPool(healPrefab, HEAL_PRELOAD_COUNT);
    }
    public GameObject GetHeal()
    {
        GameObject item = healObjectsPool.Get();
        return item;
    }
    public void ReturnHeal(GameObject coin)
    {
        healObjectsPool.Return(coin);
    }
}
