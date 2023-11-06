﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : Enemy
{
    // Main characteristics
    [SerializeField] private float AttackDistnace;
    [SerializeField] private float ShootCooldown;
    [SerializeField] private SphereCollider AttackCollider;

    public ShooterEnemyAtackState atackState { get; set; }

    //links
    [SerializeField] private Transform ShootPos; // откуда стреляем
    [SerializeField] private GameObject bullet;

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        atackState = new ShooterEnemyAtackState(this, stateMachine, AttackCollider, ShootCooldown, AttackDistnace,target);
    }
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }
    protected void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    public void Attack()
    {
        GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z)) as GameObject;
        bullet.GetComponent<Bullet>().targetPoint = target;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        if (atackState != null)
        {
            Gizmos.DrawWireSphere(transform.position, AttackDistnace);
        }
    }
}