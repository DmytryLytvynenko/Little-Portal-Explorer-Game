using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heal : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private int healAmount;
    [SerializeField] private LayerMask healLayers;
    private HealPool pool;
    private void Awake()
    {
        pool = GlobalData.healPool;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthControll healthControll))
        {
            if (((1 << collision.gameObject.layer) & healLayers) != 0)
            {
                //It matched one
                bool healed = Convert.ToBoolean(healthControll.OnHealCollected(healAmount));
                if (healed)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                    pool.ReturnHeal(gameObject);
                }
            }
        }
    }
}
