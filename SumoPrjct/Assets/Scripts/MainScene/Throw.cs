using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] private Transform throwArea;

    [SerializeField] private float xThrowSize;
    [SerializeField] private float yThrowSize;
    [SerializeField] private float zThrowSize;
    [SerializeField] private float yThrowAngle;
    [SerializeField] private float throwForce;
    [SerializeField] private int throwDamage;


    public void HeroThrow()
    {
        Collider[] overlappedColiders = Physics.OverlapBox(throwArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), throwArea.rotation);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (!rigitbody) return;
            if (!rigitbody.gameObject.CompareTag("Player")) return;

            Vector3 enemyDir = new Vector3(rigitbody.transform.position.x - transform.position.x,
                                          (rigitbody.transform.position.y - transform.position.y) + yThrowAngle,
                                           rigitbody.transform.position.z - transform.position.z);
            rigitbody.AddForce(enemyDir.normalized * throwForce, ForceMode.Impulse);
            if (rigitbody.gameObject.CompareTag("Bomb"))
            {
                rigitbody.gameObject.GetComponent<Bomb>().SetActive(true);
            }
            if (rigitbody.gameObject.layer == LayerMask.GetMask("HaveHealth"))
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
            if (!rigitbody) return;
            if (!rigitbody.gameObject.CompareTag("Boss")) return;


            Vector3 enemyDir = new Vector3(rigitbody.transform.position.x - transform.position.x,
                                          (rigitbody.transform.position.y - transform.position.y) + yThrowAngle,
                                           rigitbody.transform.position.z - transform.position.z);
            rigitbody.AddForce(enemyDir.normalized * throwForce, ForceMode.Impulse);
            if (rigitbody.gameObject.layer == LayerMask.GetMask("HaveHealth"))
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
