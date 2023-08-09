using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControll : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHelth;

    public event Action<float> HealthChanged;

    public void Start()
    {
        currentHelth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeHealth(-10);
        }
    }

    public void ChangeHealth(int value)
    {
        currentHelth += value;

        if (currentHelth <= 0)
        {
            Die();
        }
        else
        {
            float currentHealthAsPercentage = (float)currentHelth / maxHealth;
            HealthChanged?.Invoke(currentHealthAsPercentage);
        }
    }

    private void Die()
    {
        HealthChanged?.Invoke(0);
        if (this.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<HeroController>().Die();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
