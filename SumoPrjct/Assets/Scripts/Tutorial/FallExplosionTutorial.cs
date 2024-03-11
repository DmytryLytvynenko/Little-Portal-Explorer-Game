using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallExplosionTutorial : MonoBehaviour
{
    [SerializeField] private GameObject explosionTip;
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
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<HeroController>())
            return;
        if (!isTriggered)
        {
            isTriggered = true;
            explosionTip.SetActive(true);
        }
    }
}
