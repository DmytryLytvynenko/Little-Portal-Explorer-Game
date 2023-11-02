using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTargetTrigger : MonoBehaviour
{
    private Enemy parentEnemy;
    private void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            parentEnemy.IdleTargetReached?.Invoke();
        }  
    }
}
