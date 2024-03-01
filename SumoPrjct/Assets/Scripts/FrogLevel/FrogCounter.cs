using UnityEngine;
using TMPro;

public class FrogCounter : MonoBehaviour
{
    private const string QUEST_PROGRESS_DEFAULT_TEXT = "Frogs collected:";

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
        questProgressText.text = QUEST_PROGRESS_DEFAULT_TEXT + $" {FrogsAtHome} out of {FrogCount}";
        if (FrogsAtHome == FrogCount)
        {
            WinScreen.SetActive(true);
            questProgress.GetComponent<SwitchTipAnimation>().SetBool("Disappear 1");
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
