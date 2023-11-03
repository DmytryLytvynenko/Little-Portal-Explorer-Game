using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : Enemy
{
    // Main characteristics
    public float AttackDistnace;
    [SerializeField] private float xShootAngle;
    public float ShootCooldown;
    private float temporaryMoveSpeed;
    private bool isShooting;

    public ShooterEnemyAtackState atackState { get; set; }

    //links
    [SerializeField] private Transform ShootPos; // откуда стреляем
    [SerializeField] private GameObject bullet;

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        atackState = new ShooterEnemyAtackState(this, stateMachine);
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        temporaryMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (stateMachine.CurrentEnemyState == idleState)
            {
                stateMachine.ChangeState(stayState);
            }
            else
            {
                stateMachine.ChangeState(idleState);
            }
        }
/*        ControllDistance();
        Rotate();
        Move();*/
    }
    private void ControllDistance()
    {
        if (GetRotationVector(Target).magnitude <= ShootCooldown)
        {
            if (isShooting)
            {
                return;
            }
        }
    }
    private void Stop()
    {
        moveSpeed = 0;
    }
    private void Go()
    {
        moveSpeed = temporaryMoveSpeed;
    }
    public void Attack()
    {
        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = Target;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (atackState != null)
        {
            Gizmos.DrawWireSphere(transform.position, AttackDistnace);
        }
        Gizmos.color = Color.yellow;
        if (idleState != null)
        {
            Gizmos.DrawWireSphere(idleState.target.position, 0.1f);
        }
        if (chaseState != null)
        {
            Gizmos.DrawWireSphere(transform.position, AgrDistance);
        }
    }
}
