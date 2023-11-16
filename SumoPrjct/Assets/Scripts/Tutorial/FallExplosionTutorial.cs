using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallExplosionTutorial : MonoBehaviour
{
    [SerializeField] private Button explosionButton;
    [SerializeField] private GameObject explosionTip;
    [SerializeField] private GameObject obstacles;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре

    private bool isTriggered;
    private void OnEnable()
    {
        Explosion.Exploded += OnExploded;
    }
    private void OnDisable()
    {
        Explosion.Exploded -= OnExploded;
    }

    void OnExploded()
    {
        if (explosionTip.activeSelf)
        {
            explosionTip.GetComponent<Animator>().SetBool(animParameterName, true);
            explosionButton.interactable = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            explosionButton.interactable = false;
            explosionTip.SetActive(true);
            obstacles.SetActive(true);
        }
    }
}
