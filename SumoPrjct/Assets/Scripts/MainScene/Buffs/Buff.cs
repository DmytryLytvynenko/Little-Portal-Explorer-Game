using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buff : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private float newValue;
    [SerializeField] private string buffedStatName;
    [SerializeField] private float buffCooldown;
    public static Action<float, float, string> BuffCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BuffCollected?.Invoke(newValue, buffCooldown, buffedStatName);
            /*Instantiate(pickupEffect,transform.position,Quaternion.identity);*/
            Destroy(this.gameObject);
        }
    }
}
