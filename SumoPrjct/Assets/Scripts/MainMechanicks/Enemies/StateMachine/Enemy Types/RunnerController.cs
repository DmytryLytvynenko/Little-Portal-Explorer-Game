﻿using System;
using System.Collections;
using UnityEngine;

public class RunnerController : Enemy
{
    // Main characteristics
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float runDistnace;
    [SerializeField] private float runTime;
    [SerializeField] private float waitTillRunTime;
    [SerializeField] private SphereCollider attackCollider;
    public RunnerRunState atackState { get; set; }

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;
    public Action EnemyEndedRun;

    protected override void Awake()
    {
        base.Awake();

        atackState = new RunnerRunState(this, stateMachine, attackCollider, runDistnace, target);
    }
    #endregion
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindAnyObjectByType<HeroController>().transform;

        base.Start();
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
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    public IEnumerator Run(float runDelay)
    {
        Vector3 runVector;
        float timer = 0;
        float tempRotationSpeed = rotationSpeed;
        yield return new WaitForSeconds(runDelay);
        runVector = (target.position - transform.position).normalized;
        rotationSpeed -= rotationSpeed;
        while (timer < runTime)
        {
            if (rb.velocity.magnitude < maxRunSpeed)
            {
                rb.AddForce(runVector * runSpeed, ForceMode.Impulse); 
            }
            timer += Time.deltaTime;
            yield return null;
        }
        rotationSpeed = tempRotationSpeed;
        EnemyEndedRun?.Invoke();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
