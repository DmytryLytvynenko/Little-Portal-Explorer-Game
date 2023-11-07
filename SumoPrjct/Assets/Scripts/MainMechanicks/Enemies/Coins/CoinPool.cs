using ObjectsPool;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    const int COIN_PRELOAD_COUNT = 20;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObjectPool coinObjectsPool;
    private void Awake()
    {
        coinObjectsPool = new GameObjectPool(coinPrefab, COIN_PRELOAD_COUNT);
    }
    public GameObject GetCoin()
    {
        Debug.Log($"{coinObjectsPool == null}");
        GameObject item =  coinObjectsPool.Get();
        return item;
    }    
    public void ReturnCoin(GameObject coin)
    {
        coinObjectsPool.Return(coin);
    }
}
