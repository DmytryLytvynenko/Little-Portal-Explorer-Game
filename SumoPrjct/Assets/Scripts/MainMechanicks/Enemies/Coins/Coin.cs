using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private CoinPool pool;

    public static Action CoinPickedUp;

    private void Start()
    {
        pool = GameObject.FindGameObjectWithTag("CoinPool").GetComponent<CoinPool>();
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
