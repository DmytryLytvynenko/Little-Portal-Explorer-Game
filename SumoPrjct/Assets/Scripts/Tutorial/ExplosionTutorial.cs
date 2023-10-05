using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionTutorial : MonoBehaviour
{
    [SerializeField] private GameObject explosionButton;
    [SerializeField] private GameObject explosionTip;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре
    [SerializeField] private GameObject obstacles;
    [SerializeField] private HeroController heroController;

    private bool isTriggered;
    private Button btn;
    private void OnEnable()
    {
        btn = explosionButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        heroController.SetSpeedToDefault();
        explosionTip.GetComponent<Animator>().SetBool(animParameterName, true);
        btn.onClick.RemoveListener(TaskOnClick);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            explosionButton.SetActive(true);
            explosionTip.SetActive(true);
            obstacles.SetActive(true);
            heroController.SetSpeedToZero();
        }
    }
}
