using ObjectsPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPool : MonoBehaviour
{
    const int HEAL_PRELOAD_COUNT = 20;

    [SerializeField] private GameObject healPrefab;
    private GameObjectPool healObjectsPool;
    private void Awake()
    {
        healObjectsPool = new GameObjectPool(healPrefab, HEAL_PRELOAD_COUNT);
    }
    public GameObject GetHeal()
    {
        Debug.Log($"{healObjectsPool == null}");
        GameObject item = healObjectsPool.Get();
        return item;
    }
    public void ReturnHeal(GameObject coin)
    {
        healObjectsPool.Return(coin);
    }
}
