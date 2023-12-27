using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectsPool;
using System;

public class ThrowerController : Enemy
{
    const int BOMB_PRELOAD_COUNT = 5;
    const int DISK_PRELOAD_COUNT = 5;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject diskPrefab;
    [SerializeField] private Transform bombShootPoint;
    [SerializeField] private Transform diskShootPoint;
    [SerializeField] private Explosion explosion;

    [SerializeField] private SphereCollider attackCollider;
    [SerializeField] private float attackDistnace;

    [SerializeField] private float shootCooldown;
    [SerializeField] private float timeBeforeShoot;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDelay;
    [SerializeField] private int explosionDamage;

    private GameObjectPool bombObjectsPool;
    private GameObjectPool diskObjectsPool;

    public ThrowerEnemyAtackState atackState { get; set; }

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;

    #endregion

    private AttackAction randomShootAction 
    { 
        get 
        {
            if (GetRotationVector(target).magnitude < explosionRadius)
                return AttackAction.Explosion;

            int rand =  UnityEngine.Random.Range(0, 1); 
            return (AttackAction)rand; 
        }  
    }
    private enum AttackAction
    {
        Bomb,
        Disk,
        Explosion
    }

    protected override void Awake()
    {
        target =  GlobalData.PlayerInstance.transform;
        base.Awake();
        atackState = new ThrowerEnemyAtackState(this, stateMachine, shootCooldown, target);

        bombObjectsPool = new GameObjectPool(bombPrefab, BOMB_PRELOAD_COUNT);
        diskObjectsPool = new GameObjectPool(diskPrefab, DISK_PRELOAD_COUNT);

        InitializeFieldsInPool();

        rb = GetComponent<Rigidbody>();
        attackCollider.radius = attackDistnace;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public override void Move()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed * Convert.ToInt32(isGrounded), ForceMode.Impulse);//метод передвижения 
        }
    }
    public void MakeAttack()
    {
        switch (randomShootAction)
        {
            case AttackAction.Bomb:
                StartCoroutine(Attack(bombShootPoint, bombObjectsPool));
                break;
            case AttackAction.Disk:
                StartCoroutine(Attack(diskShootPoint, diskObjectsPool));
                break;
            case AttackAction.Explosion:
                StartCoroutine(ExplosionAttack(explosionDelay));
                break;
            default:
                break;
        }
    }
    private IEnumerator Attack(Transform shootPosition, GameObjectPool pool)
    {

        yield return new WaitForSeconds(timeBeforeShoot);
        /*
                ThrowerBomb bomb = Instantiate(this.bombPrefab, shootPosition.position, Quaternion.identity).GetComponent<ThrowerBomb>();
                bomb.SetThrower(shootPosition);
                bomb.SetTarget(target);
                bomb.InitiateThrow();*/

        GameObject projectileObject = pool.Get();
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.ProjectileDistroyed += OnProjectileDestroyed;


        projectile.SetThrower(shootPosition);
        projectile.SetTarget(target);
        projectile.InitiateThrow();
    }
    private IEnumerator ExplosionAttack(float explosionDelay)
    {
        yield return new WaitForSeconds(explosionDelay);

        explosion.Explode(explosionDamage);
    }
    private void OnProjectileDestroyed( Projectile projectile)
    {
        projectile.ProjectileDistroyed -= OnProjectileDestroyed;

        if (projectile.gameObject.GetComponent<ThrowerBomb>())
            bombObjectsPool.Return(projectile.gameObject);
        else
            diskObjectsPool.Return(projectile.gameObject);
    } 
    private void InitializeFieldsInPool()
    {
        List<GameObject> allItems = new List<GameObject>();
        allItems = bombObjectsPool.GetAll();
        foreach (var item in allItems)
        {
            item.GetComponent<ThrowerBomb>().SetThrower(bombShootPoint);
        }
        allItems = diskObjectsPool.GetAll();
        foreach (var item in allItems)
        {
            item.GetComponent<ThrowerDisk>().SetThrower(diskShootPoint);
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
