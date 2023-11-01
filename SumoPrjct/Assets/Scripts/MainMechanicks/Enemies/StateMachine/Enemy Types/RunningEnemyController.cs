using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningEnemyController : Enemy
{
    // Main characteristics
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
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        Target = GameObject.Find("Hero").GetComponent<Transform>();
        tempMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();
        Run();
        Rotate(Target);
        Move();
        ControllDistance();
    }
    public override void Move()
    {
        //перемещение персонажа
        if (canRun)
        {
            return;
        }
        if (rb.velocity.magnitude < maxSpeed)
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
        if (GetRotationVector(Target).magnitude <= runDistnace)
        {
            if (canRun)
            {
                return;
            }
            IndicateRun();
            moveSpeed = 0f;
            WaitForRun();
            runVector = GetRotationVector(Target);
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
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
