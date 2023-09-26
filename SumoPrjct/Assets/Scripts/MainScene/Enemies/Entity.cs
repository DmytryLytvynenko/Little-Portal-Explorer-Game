using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Transform target;
    protected Rigidbody rb;
    [SerializeField] protected int contactDamage;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float rotationSpeed;

    protected Vector3 rotationVector// направление  передвижения
    {
        get
        {
            return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z);
        }
    }
    protected void GiveContactDamage(Collision collision)
    {
        if (!collision.gameObject.GetComponent<HealthControll>())
        {
            return;
        }
        else
        {
            collision.gameObject.GetComponent<HealthControll>().ChangeHealth(-contactDamage);
        }
    }
    protected void Die()
    {
        Destroy(this.gameObject);
    }
    protected virtual void Move()
    {
        //перемещение персонажа

        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!this.enabled)
        {
            this.enabled = true;
        }
    }
}
