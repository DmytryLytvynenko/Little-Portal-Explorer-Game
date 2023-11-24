using UnityEngine;

public class ChasingEnemyController : Enemy
{
    protected override void Awake()
    {
        target = GlobalData.PlayerInstance.transform;
        base.Awake();
    }
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
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
