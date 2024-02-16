using System;
using UnityEngine;

public class BomberController : Enemy
{
    // Main characteristics
    [SerializeField] private float attackDistnace;
    [SerializeField] private float ShootCooldown;
    [SerializeField] private SphereCollider AttackCollider;

    public BomberEnemyAtackState atackState { get; set; }

    //links
    [SerializeField] private Transform ShootPos; // откуда стреляем
    [SerializeField] private GameObject bomb;

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;

    #endregion

    protected override void Awake()
    {
        target = GlobalData.PlayerInstance.transform;
        base.Awake();

        atackState = new BomberEnemyAtackState(this, stateMachine, AttackCollider, ShootCooldown, attackDistnace, target);
    }
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }
    protected void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    protected void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public void Attack()
    {
        ThrowerBomb bomb = Instantiate(this.bomb, ShootPos.position, Quaternion.identity).GetComponent<ThrowerBomb>();
        bomb.SetThrower(ShootPos);
        bomb.SetTarget(target);
        bomb.InitiateThrow();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        if (atackState != null)
        {
            Gizmos.DrawWireSphere(transform.position, attackDistnace);
        }
    }
}

