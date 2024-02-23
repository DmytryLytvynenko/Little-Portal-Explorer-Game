using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private GameObject enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            enemies.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
