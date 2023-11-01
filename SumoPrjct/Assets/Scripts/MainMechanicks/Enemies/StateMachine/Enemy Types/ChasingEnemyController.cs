using UnityEngine;

public class ChasingEnemyController : Enemy
{
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        Rotate(GetRotationVectorChase());
        Move();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
