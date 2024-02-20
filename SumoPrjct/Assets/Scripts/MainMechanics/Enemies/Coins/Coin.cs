using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
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
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            CoinPickedUp?.Invoke();
            pool.ReturnCoin(gameObject);
        }
    }
}
