using UnityEngine;

public class ChasingEnemyController : Enemy
{
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Rotate(Target);
        Move();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
