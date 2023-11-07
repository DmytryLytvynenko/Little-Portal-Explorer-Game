using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int coinCount { get; private set; } = 0;

    private void OnEnable()
    {
        Coin.CoinPickedUp += OnCoinPickedUp;
    }
    private void OnDisable()
    {
        Coin.CoinPickedUp -= OnCoinPickedUp;
    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log(coinCount);
    }
    public void SaveCoins() 
    { 

    }
    private void OnCoinPickedUp()
    {
        AddCoin();
    }
}
