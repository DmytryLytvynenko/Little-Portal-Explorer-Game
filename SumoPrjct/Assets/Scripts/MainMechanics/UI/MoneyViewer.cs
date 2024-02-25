using TMPro;
using UnityEngine;

public class MoneyViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCount;

    public void UpdateCoinCount(int _coinCount)
    {
        coinCount.text = _coinCount.ToString();
    }
}
