using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBagMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            canvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            canvas.SetActive(false);
        }
    }
}
