using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectsPool;
public class ThrowerEnemy : Entity
{
    const int BOMB_PRELOAD_COUNT = 5;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject diskPrefab;
    [SerializeField] private Transform bombShootPoint;
    [SerializeField] private Transform diskShootPoint;
    [SerializeField] private float shootDistnace;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float timeBeforeShoot;
    private float temporaryMoveSpeed;
    private bool isShooting;
    private ThrowerBomb bomb;
    private ThrowerDisk disk;

    private GameObjectPool bombObjectsPool;

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

    private void Awake()
    {
        bombObjectsPool = new GameObjectPool(bombPrefab, BOMB_PRELOAD_COUNT);

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

            StartCoroutine(Shoot( bombShootPoint));
/*            switch (randomShootAction)
            {
                case ShootAction.Bomb:
                    StartCoroutine(Shoot(bomb, bombShootPoint));
                    break;
                case ShootAction.Disk:
                    StartCoroutine(Shoot(disk, diskShootPoint));
                    break;
                default:
                    break;
            }*/
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
    private IEnumerator Shoot(Transform shootPosition)
    {
        Stop();
        isShooting = true;

        yield return new WaitForSeconds(timeBeforeShoot);

        GameObject bombObject = bombObjectsPool.Get();
        ThrowerBomb throwerBomb = bombObject.GetComponent<ThrowerBomb>();
        throwerBomb.ThrowersBombExploded += OnThrowersBombExploded;

        bombObject.transform.position = shootPosition.position;

        throwerBomb.InitiateThrow();


        yield return new WaitForSeconds(shootCooldown);
        Go();
        isShooting = false;
    }

    private void OnThrowersBombExploded( ThrowerBomb throwerBomb)
    {
        throwerBomb.ThrowersBombExploded -= OnThrowersBombExploded;
        bombObjectsPool.Return(throwerBomb.gameObject);
    }
    private void InitializeFieldsInPool()
    {
        List<GameObject> allItems = new List<GameObject>();
        allItems = bombObjectsPool.GetAll();
        foreach (var item in allItems)
        {
            item.GetComponent<ThrowerBomb>().SetThrower(bombShootPoint);
        }
    }
}
