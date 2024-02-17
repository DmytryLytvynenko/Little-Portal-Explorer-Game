using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heal : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private int healAmount;
    private HealPool pool;
    private void Awake()
    {
        pool = GlobalData.healPool;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthControll healthControll))
        {
            bool healed = Convert.ToBoolean(healthControll.OnHealCollected(healAmount));
            if (healed)
            {
                /*Instantiate(pickupEffect,transform.position,Quaternion.identity);*/
                pool.ReturnHeal(gameObject);
            }
        }
    }
}
