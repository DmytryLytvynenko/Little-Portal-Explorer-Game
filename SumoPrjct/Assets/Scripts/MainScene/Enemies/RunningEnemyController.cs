using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningEnemyController : Enemy
{
    // Main characteristics
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float runDistnace;
    [SerializeField] private float runTime;
    [SerializeField] private float waitTillRunTime;

    private Vector3 runVector;
    private bool canRun;
    private float timer = 0f;
    private float timer1 = 0f;
    private float tempMoveSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
        tempMoveSpeed = moveSpeed;
    }
    void Update()
    {
        Run();
        Move();
        ControllDistance();
    }
    protected override void Move()
    {
        //перемещение персонажа
        if (canRun)
        {
            return;
        }

        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        if (rb.velocity.magnitude < maxMoveSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    private void Run()
    {
        if (!canRun)
        {
            return;
        }
        if (timer >= runTime)
        {
            timer = 0f;
            canRun = false;
            DeIndicateRun();
            moveSpeed = tempMoveSpeed;
            return;
        }
        if (rb.velocity.magnitude < maxRunSpeed)
        {
            rb.AddForce(runVector.normalized * runSpeed, ForceMode.Impulse); 
        }
        timer += Time.deltaTime;
    }
    private void ControllDistance()
    {
        if (rotationVector.magnitude <= runDistnace)
        {
            if (canRun)
            {
                return;
            }
            IndicateRun();
            moveSpeed = 0f;
            WaitForRun();
            runVector = rotationVector;
        }
        else
        {
            if (canRun)
            {
                return;
            }
            if (moveSpeed != 0)
            {
                return;
            }
            DeIndicateRun();
            moveSpeed = tempMoveSpeed;
            timer1 = 0f;
        }
    }
    private void IndicateRun()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
    private void DeIndicateRun()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    private void WaitForRun()
    {
        if (timer1 <= waitTillRunTime)
        {
            timer1 += Time.deltaTime;
        }
        else
        {
            timer1 = 0f;
            canRun = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GiveContactDamage(collision);
    }
}
