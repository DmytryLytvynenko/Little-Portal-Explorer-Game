using Sound_Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControll : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHelth;

    [Header("Sound Player")]
    [SerializeField] private SoundPlayer soundPlayer;

    public event Action<float> HealthChanged;
    public event Action<Transform> DamageTaken;
    public event Action EntityDied;

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
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

    public void ChangeHealth(int value, Transform damager = null)
    {
        if (value < 0)
            DamageTaken?.Invoke(damager);

        currentHelth += value;
        if (currentHelth > maxHealth)
        {
            currentHelth = maxHealth;
        }
        if (currentHelth <= 0)
        {
            Die();
            return;
        }

        float currentHealthAsPercentage = (float)currentHelth / maxHealth;
        HealthChanged?.Invoke(currentHealthAsPercentage);
    }
    public bool OnHealCollected(int healAmount)
    {
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
        soundPlayer.PlaySound(SoundName.Die);
        EntityDied?.Invoke();
    }
}
