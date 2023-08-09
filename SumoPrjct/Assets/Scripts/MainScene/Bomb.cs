using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int explosionDamage;
    private bool isActive = false;
    private Explosion explosion;
    void Start()
    {
        explosion = GetComponent<Explosion>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            explosion.Explode(explosionDamage);
            Destroy(this.gameObject);
        }
    }
    public void SetActive(bool status)
    {
        isActive = status;
    }
}
