using UnityEngine;
using TMPro;

public class FrogCounter : MonoBehaviour
{
    private const string QUEST_PROGRESS_DEFAULT_TEXT = "Ћ€гушек собрано:";

    [SerializeField] private FrogSpawner spawner;
    [SerializeField] private GameObject questProgress;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private TextMeshProUGUI questProgressText;

    private int FrogsAtHome = 0;
    private int FrogCount;
    private void Start()
    {
        FrogCount = spawner.FrogCount;
        InvokeRepeating(nameof(UpdateQuestText),0f,2f);
    }
    private void UpdateQuestText()
    {
        questProgressText.text = QUEST_PROGRESS_DEFAULT_TEXT + $" {FrogsAtHome} из {FrogCount}";
        if (FrogsAtHome == FrogCount)
        {
            WinScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        FrogDisabler.FrogArrivedHome += OnFrogArrivedHome;
    }
    private void OnDisable()
    {
        FrogDisabler.FrogArrivedHome -= OnFrogArrivedHome;
    }
    private void OnFrogArrivedHome()
    {
        FrogsAtHome += 1;
    }
}
