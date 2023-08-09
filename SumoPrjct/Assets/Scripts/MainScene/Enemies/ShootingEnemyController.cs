using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : Enemy
{
    // Main characteristics
    private float temporaryMoveSpeed;
    public float shootDistnace;
    public float xShootAngle;
    public float shootCooldown;
    private float timer = 0;

    //links
    private Transform ShootPos; // откуда стреляем
    public GameObject bullet;

    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
        ShootPos = this.gameObject.transform.GetChild(0);

        temporaryMoveSpeed = moveSpeed;
    }
    void Update()
    {
        ControllDistance();
        Move();
    }

    private void ControllDistance()
    {
        if (rotationVector.magnitude <= shootDistnace)
        {
            Stop();
            if (timer >= shootCooldown)
            {
                Shoot();
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            Go();
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
    private void Shoot()
    {
        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = target;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GiveContactDamage(collision);
    }
}
