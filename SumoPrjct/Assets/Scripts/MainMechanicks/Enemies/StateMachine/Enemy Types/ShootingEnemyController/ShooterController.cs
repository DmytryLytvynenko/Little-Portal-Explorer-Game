using System;
using UnityEngine;
using Sound_Player;

public enum ShooterAnimParametrs 
{
    Attack
}
public class ShooterController : Enemy
{
    // Main characteristics
    [SerializeField] private float attackDistnace;
    [SerializeField] private float ShootCooldown;
    [SerializeField] private SphereCollider AttackCollider;

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
        target = GlobalData.PlayerInstance.transform;
        base.Awake();

        atackState = new ShooterEnemyAtackState(this, stateMachine, AttackCollider, ShootCooldown, attackDistnace,target,animator);
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
        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = target;
        soundEffectPlayer.PlaySound(SoundName.Attack);
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
