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
    public event Action DamageTaken;
    public event Action EntityDied;

    private void OnEnable()
    {
        Heal.HealCollected += OnHealCollected;
    }
    private void OnDisable()
    {
        Heal.HealCollected -= OnHealCollected;
    }

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
        if (value < 0)
            DamageTaken?.Invoke();

        currentHelth += value;
        if (currentHelth > maxHealth)
        {
            currentHelth -= value;
            return;
        }
        if (currentHelth <= 0)
        {
            Die();
            return;
        }

        float currentHealthAsPercentage = (float)currentHelth / maxHealth;
        HealthChanged?.Invoke(currentHealthAsPercentage);
    }
    private bool OnHealCollected(int healAmount)
    {
        if (!gameObject.CompareTag("Player"))
            return false;

        if (currentHelth == maxHealth)
            return false;

        ChangeHealth(healAmount);
        return true;
    }
    public void RestoreHealth()
    {
        ChangeHealth(maxHealth - currentHelth);
    }
    private void Die()
    {
        HealthChanged?.Invoke(0);
        if (this.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<HeroController>().Die();
        }

        EntityDied?.Invoke();
    }
}
