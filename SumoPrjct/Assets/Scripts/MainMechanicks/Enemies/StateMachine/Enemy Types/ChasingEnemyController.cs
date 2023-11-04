using UnityEngine;

public class ChasingEnemyController : Enemy
{
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        base.Start();
    }
    protected void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    protected void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }
}
