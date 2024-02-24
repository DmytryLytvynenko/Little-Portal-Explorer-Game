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
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;

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
    public bool NotFull 
    {
        get { return currentHelth != maxHealth; }
    }
    public void ChangeHealth(int value, Transform damager = null)
    {
        currentHelth += value;
        if (value >= 0) 
        {
            if (currentHelth > maxHealth)
                currentHelth = maxHealth;

            if (value != 0)
                soundEffectPlayer.PlaySound(SoundName.Heal);
        }
        else
        {
            DamageTaken?.Invoke(damager);
            if (currentHelth <= 0)
            {
                soundEffectPlayer.PlaySound(SoundName.Die);
                Die();
            }
            else
            {
                soundEffectPlayer.PlaySound(SoundName.TakeDamage);
            }
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
        soundEffectPlayer.PlaySound(SoundName.Die);
        EntityDied?.Invoke();
    }
}
