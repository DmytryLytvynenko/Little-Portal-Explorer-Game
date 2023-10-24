using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectsPool;
public class ThrowerEnemy : Entity
{
    const int BOMB_PRELOAD_COUNT = 5;
    const int DISK_PRELOAD_COUNT = 5;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject diskPrefab;
    [SerializeField] private Transform bombShootPoint;
    [SerializeField] private Transform diskShootPoint;
    [SerializeField] private float shootDistnace;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float bombDistance;
    [SerializeField] private float timeBeforeShoot;
    private float temporaryMoveSpeed;
    private bool isShooting;

    private GameObjectPool bombObjectsPool;
    private GameObjectPool diskObjectsPool;

    private ShootAction randomShootAction 
    { 
        get 
        {
            if (rotationVector.magnitude < bombDistance)
            {
                return ShootAction.Disk;
            }
            int rand =  UnityEngine.Random.Range(0, 2); 
            return (ShootAction)rand; 
        }  
    }
    private enum ShootAction
    {
        Bomb,
        Disk
    }

    private void Awake()
    {
        bombObjectsPool = new GameObjectPool(bombPrefab, BOMB_PRELOAD_COUNT);
        diskObjectsPool = new GameObjectPool(diskPrefab, DISK_PRELOAD_COUNT);

        InitializeFieldsInPool();

        temporaryMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindAnyObjectByType<HeroController>().transform;
    }
    void Update()
    {
        ControllDistance();
        Rotate();
        Move();
    }
    private void ControllDistance()
    {
        if (rotationVector.magnitude <= shootDistnace)
        {
            if (isShooting)
                return;

            switch (randomShootAction)
            {
                case ShootAction.Bomb:
                    StartCoroutine(Shoot(bombShootPoint, bombObjectsPool));
                    break;
                case ShootAction.Disk:
                    StartCoroutine(Shoot(diskShootPoint, diskObjectsPool));
                    break;
                default:
                    break;
            }
        }
    }
    private void Stop()
    {
        moveSpeed = 0;
        rb.velocity = Vector3.zero;
    }
    private void Go()
    {
        moveSpeed = temporaryMoveSpeed;
    }
    private IEnumerator Shoot(Transform shootPosition, GameObjectPool pool)
    {
        Stop();
        isShooting = true;

        yield return new WaitForSeconds(timeBeforeShoot);

        GameObject projectileObject = pool.Get();
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.ProjectileDistroyed += OnProjectileDestroyed;

        projectileObject.transform.position = shootPosition.position;

        projectile.InitiateThrow();


        yield return new WaitForSeconds(shootCooldown);
        Go();
        isShooting = false;
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
