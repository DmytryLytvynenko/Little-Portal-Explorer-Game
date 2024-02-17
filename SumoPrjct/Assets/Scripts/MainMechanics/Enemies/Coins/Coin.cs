using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinPool pool;

    public static Action CoinPickedUp;

    private void Start()
    {
        pool = GlobalData.coinPool;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>()) 
        {
            CoinPickedUp?.Invoke();
            pool.ReturnCoin(gameObject);
        }
    }
}
