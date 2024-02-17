using UnityEngine;
using Utilities;

public class GlobalDataInitializer : MonoBehaviour
{
    private void Awake()
    {
        GlobalData.PlayerInstance = FindFirstObjectByType<HeroController>();
        GlobalData.healPool = FindFirstObjectByType<HealPool>();
        GlobalData.coinPool = FindFirstObjectByType<CoinPool>();
    }
}
