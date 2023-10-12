using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heal : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private int healAmount;
    public static Func<int, bool> HealCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool healed = Convert.ToBoolean(HealCollected?.Invoke(healAmount));
            if (healed)
            {
                /*Instantiate(pickupEffect,transform.position,Quaternion.identity);*/
                Destroy(this.gameObject);
            }
        }
    }
}
