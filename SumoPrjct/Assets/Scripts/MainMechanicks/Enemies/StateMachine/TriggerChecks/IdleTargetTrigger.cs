using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTargetTrigger : MonoBehaviour
{
    [SerializeField] private Enemy parentEnemy;
    private void OnTriggerEnter(Collider other)
    {
        if (parentEnemy.IdleTargetTrigger == this)
        {
            parentEnemy.IdleTargetReached?.Invoke();
        }  
    }
}
