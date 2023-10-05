using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buff : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private float newValue;
    [SerializeField] private string buffedStatName;
    public static Action<float, string> BuffCollected;

    private void OnTriggerEnter(Collider other)
    {
        BuffCollected?.Invoke(newValue, buffedStatName);
        /*Instantiate(pickupEffect,transform.position,Quaternion.identity);*/
        Destroy(this.gameObject);
    }
}
