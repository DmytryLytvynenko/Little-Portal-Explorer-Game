using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int explosionDamage;
    [SerializeField] private HealthControll healthControll;
    private bool isActive = false;
    private Explosion explosion;
    private void OnEnable()
    {
        healthControll.EntityDied += OnEntityDied;
    }
    private void OnDisable()
    {
        healthControll.EntityDied -= OnEntityDied;
    }
    void Start()
    {
        explosion = GetComponent<Explosion>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            isActive = false;
            GetComponent<MeshRenderer>().enabled = false;
            explosion.Explode(explosionDamage);
            Invoke(nameof(Die), 1.5f);
        }
    }
    public void SetActive(bool status)
    {
        isActive = status;
    }
    protected virtual void OnEntityDied()
    {
        GetComponent<MeshRenderer>().enabled = false;
        explosion.Explode(explosionDamage);
        Invoke(nameof(Die), 1.5f);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
