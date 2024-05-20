using Sound_Player;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

public class Wallet : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
    [SerializeField] private MoneyViewer moneyViewer;
    private PhotonView PV = null;
    public int coinCount { get; private set; } = 0;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV == null) return;
        if (!PV.IsMine) return;

        coinCount = PlayerPrefs.GetInt("PlayersMoney");
        moneyViewer.UpdateCoinCount(coinCount);
    }
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
        if (PV == null) return;
        if (!PV.IsMine) return;

        coinCount++;
        moneyViewer.UpdateCoinCount(coinCount);
        Debug.Log("CoinCount = " + coinCount);
    }
    public void SpendCoin( int coinAmount)
    {
        coinCount -= coinAmount;
        moneyViewer.UpdateCoinCount(coinCount);
        Debug.Log("CoinCount = " + coinCount);
    }
    public void SetMoneyViewer(MoneyViewer _moneyViewer)
    {
        moneyViewer = _moneyViewer;
    }
    public bool HasMoneyViewer()
    {
        return moneyViewer;
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

    [Button("DeleteMoney")]
    private void DeleteMoney()
    {
        PlayerPrefs.DeleteKey("PlayersMoney");
    }
}
