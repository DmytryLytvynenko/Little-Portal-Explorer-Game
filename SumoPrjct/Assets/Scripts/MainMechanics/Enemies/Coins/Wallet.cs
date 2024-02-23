using Sound_Player;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
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
        Debug.Log("CoinCount = " + coinCount);
    }
    public void SaveCoins() 
    {
        PlayerPrefs.SetInt("PlayersMoney", coinCount);
    }
    private void OnCoinPickedUp()
    {
        soundEffectPlayer.PlaySound(SoundName.Coin);
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
