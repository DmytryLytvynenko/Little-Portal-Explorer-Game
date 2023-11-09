using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float noDamageExplosionForce;
    [SerializeField] private float noDamageExplosionRadius;
    [SerializeField] private GameObject explosionEffect;
    public static Action Exploded;

    private EffectScaling effectScaling;
    private float defaultExplosionRadius;
    private void Awake()
    {
        defaultExplosionRadius = explosionRadius;
        effectScaling = explosionEffect.GetComponent<EffectScaling>();
        effectScaling.SetScale(explosionRadius);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))// потом убрать
        {
            Explode(3000);
        }
    }
    public void SetExplosionRadius(float newRadius)
    {
        explosionRadius = newRadius;
        effectScaling.SetScale(newRadius);
        print($"Scale now{newRadius}");
    }    
    public void SetExplosionRadiusToNormal()
    {
        explosionRadius = defaultExplosionRadius;
        effectScaling.SetScale(explosionRadius);
    }
    public void Explode(int explosionDamage)
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody == null)
            {
                continue;
            }
            if (rigitbody.CompareTag("Bullet"))
            {
                continue;
            }
            if (rigitbody.gameObject.layer == gameObject.layer)
            {
                continue;
            }
            Vector3 distanceToTarget = new Vector3(transform.position.x - rigitbody.transform.position.x, transform.position.y - rigitbody.transform.position.y, transform.position.z - rigitbody.transform.position.z);
            int damage =Convert.ToInt32(((explosionRadius - distanceToTarget.magnitude) / explosionRadius) * explosionDamage);// Чем ближе к игроку протимник тем больше damage
            if (rigitbody.gameObject.CompareTag("Bomb") && damage >= (explosionDamage/2/*магическое число*/))    // Активируем бомбу если она задета взрывом достаточно сильно
            {
                rigitbody.gameObject.GetComponent<Bomb>().SetActive(true);
            }
            rigitbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            HealthControll healthControll;
            if (rigitbody.TryGetComponent(out healthControll))
            {
                healthControll.ChangeHealth(-damage);
            }
        }
        Exploded?.Invoke();

        Ray ray = new Ray(transform.position, -100 * Vector3.up);
        Physics.Raycast(ray, out RaycastHit hit);
        Instantiate(explosionEffect,new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z), Quaternion.identity);
    }
    public void NoDamageExplode()
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, noDamageExplosionRadius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody == null)
            {
                continue;
            }
            if (rigitbody.CompareTag("Bullet"))
            {
                continue;
            }
            if (rigitbody.CompareTag("Player"))
            {
                continue;
            }
            rigitbody.AddExplosionForce(noDamageExplosionForce, transform.position, noDamageExplosionRadius);
        }
        /*Instantiate(explosionEffect, transform.position, Quaternion.identity);*/
    }
    public void BossExplode(int explosionDamage)
    {
        Collider[] overlappedColiders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody == null)
            {
                continue;
            }
            if (rigitbody.CompareTag("Bullet"))
            {
                continue;
            }
            if (rigitbody.CompareTag("Boss"))
            {
                continue;
            }
            Vector3 distanceToTarget = new Vector3(transform.position.x - rigitbody.transform.position.x, transform.position.y - rigitbody.transform.position.y, transform.position.z - rigitbody.transform.position.z);
            int damage = Convert.ToInt32(((explosionRadius - distanceToTarget.magnitude) / explosionRadius) * explosionDamage);
            rigitbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            if (rigitbody.gameObject.layer == LayerMask.NameToLayer("HaveHealth"))
            {
                rigitbody.GetComponent<HealthControll>().ChangeHealth(-damage);
            }
        }
        /*Instantiate(explosionEffect, transform.position, Quaternion.identity);*/
    }
}
