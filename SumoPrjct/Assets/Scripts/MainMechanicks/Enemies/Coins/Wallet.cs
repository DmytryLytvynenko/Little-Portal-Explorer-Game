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
        SaveCoins();
    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log(coinCount);
    }
    public void SaveCoins() 
    {
        PlayerPrefs.SetInt("PlayersMoney", coinCount);
    }
    private void OnCoinPickedUp()
    {
        AddCoin();
    }

    private void OnDestroy()
    {
        SaveCoins();
    }
    private void OnApplicationQuit()
    {
        SaveCoins();
    }
}
