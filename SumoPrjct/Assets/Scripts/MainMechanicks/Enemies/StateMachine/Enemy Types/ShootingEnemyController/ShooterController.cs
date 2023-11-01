using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : Enemy
{
    // Main characteristics
    [SerializeField] private float shootDistnace;
    [SerializeField] private float xShootAngle;
    [SerializeField] private float shootCooldown;
    private float temporaryMoveSpeed;
    private bool isShooting;

    public EnemyAtackState atackState { get; set; }

    //links
    [SerializeField] private Transform ShootPos; // откуда стреляем
    [SerializeField] private GameObject bullet;

    protected override void Awake()
    {
        base.Awake();

        atackState = new EnemyAtackState(this, stateMachine);
    }
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
/*        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();*/

        temporaryMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();

/*        ControllDistance();
        Rotate();
        Move();*/
    }
    private void ControllDistance()
    {
        if (GetRotationVector(Target).magnitude <= shootDistnace)
        {
            if (isShooting)
            {
                return;
            }
            StartCoroutine(Shoot());
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
    public override void Attack() 
    {
        StartCoroutine(Shoot());
    }
    private IEnumerator Shoot()
    {
        isShooting = true;
        Stop();

        yield return new WaitForSeconds(shootCooldown);

        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = Target;

        isShooting = false;
        Go();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
