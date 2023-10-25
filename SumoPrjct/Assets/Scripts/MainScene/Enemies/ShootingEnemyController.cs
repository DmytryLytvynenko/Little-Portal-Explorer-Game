using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : Entity
{
    // Main characteristics
    [SerializeField] private float shootDistnace;
    [SerializeField] private float xShootAngle;
    [SerializeField] private float shootCooldown;
    private float temporaryMoveSpeed;
    private bool isShooting;

    //links
    [SerializeField] private Transform ShootPos; // откуда стреляем
    [SerializeField] private GameObject bullet;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        temporaryMoveSpeed = moveSpeed;
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
    private IEnumerator Shoot()
    {
        isShooting = true;
        Stop();

        yield return new WaitForSeconds(shootCooldown);

        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = target;

        isShooting = false;
        Go();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
