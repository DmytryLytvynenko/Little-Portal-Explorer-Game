using UnityEngine;

public class ChasingEnemyController : Entity
{
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        Move();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        GiveContactDamage(collision);
    }
}
