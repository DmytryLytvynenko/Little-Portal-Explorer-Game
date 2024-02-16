using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heal : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthControll healthControll))
        {
            bool healed = Convert.ToBoolean(healthControll.OnHealCollected(healAmount));
            if (healed)
            {
                /*Instantiate(pickupEffect,transform.position,Quaternion.identity);*/
                Destroy(this.gameObject);
            }
        }
    }
}
