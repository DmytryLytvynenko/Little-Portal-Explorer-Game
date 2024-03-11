using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionTutorial : MonoBehaviour
{
    [SerializeField] private GameObject explosionTip;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре
    [SerializeField] private GameObject obstacles;
    [SerializeField] private HeroController heroController;

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
        heroController.SetSpeedToDefault();
        explosionTip.GetComponent<Animator>().SetBool(animParameterName, true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            explosionTip.SetActive(true);
            obstacles.SetActive(true);
            heroController.SetSpeedToZero();
        }
    }
}
