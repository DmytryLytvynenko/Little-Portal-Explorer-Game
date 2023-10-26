using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    protected Transform target;
    protected Rigidbody rb;
    [SerializeField] protected int contactDamage;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float rotationSpeed;
    protected bool isGrounded;

    public static Action<int> EnemyAndPlayerContacted;

    protected Vector3 rotationVector// направление  передвижения
    {
        get
        {
            return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z);
        }
    }
    protected virtual void Rotate()
    {
        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
    protected virtual void Move()
    {
        //перемещение персонажа

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed * Convert.ToInt32(isGrounded), ForceMode.Impulse);//метод передвижения 
        }
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//magic word!!!
            EnemyAndPlayerContacted?.Invoke(contactDamage);

        if (collision.gameObject.CompareTag("Ground"))//magic word!!!
            isGrounded = true;
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))//magic word!!!
        {
            isGrounded = false;
        }
    }
    protected void Die()
    {
        Destroy(this.gameObject);
    }
}
