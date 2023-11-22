using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] private Transform throwArea;

    [Range(0f, 7f)]
    [SerializeField] private float yThrowAngle;

    [SerializeField] private float throwForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private int throwDamage;
    [SerializeField] private LayerMask throwLayers;

    private float xThrowSize;
    private float yThrowSize;
    private float zThrowSize;

    private void Start()
    {
        xThrowSize = throwArea.localScale.x;
        yThrowSize = throwArea.localScale.y;
        zThrowSize = throwArea.localScale.z;
    }

    public void HeroThrow()
    {
        Collider[] overlappedColiders = Physics.OverlapBox(throwArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), throwArea.rotation, throwLayers);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (!rigitbody) continue;
            if (rigitbody.gameObject == this.gameObject) continue;

            rigitbody.AddExplosionForce(throwForce, new Vector3(transform.position.x, rigitbody.position.y - yThrowAngle, transform.position.z), explosionRadius);
            Debug.DrawLine(new Vector3(transform.position.x, rigitbody.position.y - yThrowAngle, transform.position.z), rigitbody.position,Color.red, 100f);
            if (rigitbody.gameObject.CompareTag("Bomb"))
            {
                rigitbody.gameObject.GetComponent<Bomb>().SetActive(true);
            }
            if (rigitbody.gameObject.layer == LayerMask.NameToLayer("HaveHealth"))
            {
                rigitbody.GetComponent<HealthControll>().ChangeHealth(-throwDamage);
            }
        }
    }
    public void EnemyBossThrow()
    {
        Collider[] overlappedColiders = Physics.OverlapBox(throwArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), throwArea.rotation);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (!rigitbody) continue;
            if (rigitbody.gameObject.CompareTag("Boss")) continue;


            Vector3 enemyDir = new Vector3(rigitbody.transform.position.x - transform.position.x,
                                          (rigitbody.transform.position.y - transform.position.y) + yThrowAngle,
                                           rigitbody.transform.position.z - transform.position.z);
            rigitbody.AddForce(enemyDir.normalized * throwForce, ForceMode.Impulse);
            if (rigitbody.gameObject.layer == LayerMask.NameToLayer("HaveHealth"))
            {
                rigitbody.GetComponent<HealthControll>().ChangeHealth(-throwDamage);
            }
        }
    }
    private void Update()
    {
        VisualizeBox.DisplayBox(throwArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), throwArea.rotation);
    }
}
