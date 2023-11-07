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

    [SerializeField] private SphereCollider attackCollider;
    [SerializeField] private float attackDistnace;

    [SerializeField] private float shootCooldown;
    [SerializeField] private float timeBeforeShoot;

    private GameObjectPool bombObjectsPool;
    private GameObjectPool diskObjectsPool;

    public ThrowerEnemyAtackState atackState { get; set; }

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;

    #endregion

    private ShootAction randomShootAction 
    { 
        get 
        {
            int rand =  UnityEngine.Random.Range(0, 2); 
            return (ShootAction)rand; 
        }  
    }
    private enum ShootAction
    {
        Bomb,
        Disk
    }

    protected override void Awake()
    {
        target =  GlobalData.PlayerInstance.transform;
        base.Awake();

        bombObjectsPool = new GameObjectPool(bombPrefab, BOMB_PRELOAD_COUNT);
        diskObjectsPool = new GameObjectPool(diskPrefab, DISK_PRELOAD_COUNT);

        InitializeFieldsInPool();

        rb = GetComponent<Rigidbody>();
        attackCollider.radius = attackDistnace;
        atackState = new ThrowerEnemyAtackState(this, stateMachine, shootCooldown, target);
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
            case ShootAction.Bomb:
                StartCoroutine(Attack(bombShootPoint, bombObjectsPool));
                break;
            case ShootAction.Disk:
                StartCoroutine(Attack(diskShootPoint, diskObjectsPool));
                break;
            default:
                break;
        }
    }
    private IEnumerator Attack(Transform shootPosition, GameObjectPool pool)
    {

        yield return new WaitForSeconds(timeBeforeShoot);

        GameObject projectileObject = pool.Get();
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.ProjectileDistroyed += OnProjectileDestroyed;

        projectileObject.transform.position = shootPosition.position;

        projectile.InitiateThrow();
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
}
