using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShooterEnemyAtackState : EnemyState
{
    private Transform target;
    private ShooterController shooter;
    private Action ExitAttackStateFunc;
    private Coroutine ExitAttackStateCoroutine;
    private SphereCollider attackCollider;
    private float ExitAttackStateDelay = 2f;
    private float shootCooldown;
    private float timer = 0;

    public ShooterEnemyAtackState(ShooterController enemy, EnemyStateMachine enemyStateMachine,SphereCollider AttackCollider,float ShootCooldown,float AttackDistnace, Transform Target) : base(enemy, enemyStateMachine)
    {
        target = Target;
        attackCollider = AttackCollider;
        AttackCollider.radius = AttackDistnace;
        shooter = enemy;
        shootCooldown = ShootCooldown;
        ExitAttackStateFunc = ExitAttackState;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }
    public override void OnEnable()
    {
        shooter.PlayerEnteredAttackZone += OnPlayerEnteredAttackZone;
        shooter.PlayerExitedAttackZone += OnPlayerExitedAttackZone;
    }

    public override void OnDisable()
    {
        shooter.PlayerEnteredAttackZone -= OnPlayerEnteredAttackZone;
        shooter.PlayerExitedAttackZone -= OnPlayerExitedAttackZone;
    }
    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        CountTimeToShoot();
        enemy.Rotate(target);
    }

    private void OnPlayerEnteredAttackZone()
    {
        if (ExitAttackStateCoroutine != null)
        {
            Coroutines.StopRoutine(ExitAttackStateCoroutine);
            ExitAttackStateCoroutine = null;
            return;
        }
        if (enemyStateMachine.CurrentEnemyState == this)
            return;

        enemyStateMachine.ChangeState(this);
        Debug.Log(timer);
    }
    private void OnPlayerExitedAttackZone()
    {
        if (ExitAttackStateCoroutine != null)
            return;

        ExitAttackStateCoroutine = Coroutines.StartRoutine(Utilities.Utilities.DoMethodAftedDelay(ExitAttackStateFunc, ExitAttackStateDelay));

    }
    private void ExitAttackState()
    {
        enemyStateMachine.ChangeState(enemy.chaseState);
        ExitAttackStateCoroutine = null;
    }
    private void CountTimeToShoot()
    {
        timer += Time.deltaTime;
        if (timer > shootCooldown) 
        {
            shooter.Attack();
            timer = 0;
        }
    }
}
