using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTargetTrigger : MonoBehaviour
{
    private Enemy parentEnemy;
    private void Awake()
    {
        parentEnemy = GetComponentInParent<Enemy>();
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parentEnemy.gameObject)
        {
            parentEnemy.IdleTargetReached?.Invoke();
        }  
    }
}
