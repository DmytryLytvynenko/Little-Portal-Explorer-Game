using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShooterEnemyAtackState : EnemyState
{
    ShooterController shooter;
    private Action ExitAttackStateFunc;
    private float ExitAttackStateDelay = 2f;
    private Coroutine ExitAttackStateCoroutine;
    private float timer = 0;

    public ShooterEnemyAtackState(ShooterController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        shooter = enemy;
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
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        CountTimeToShoot();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.Rotate(enemy.Target);
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
    }
    private void OnPlayerExitedAttackZone()
    {
        if (ExitAttackStateCoroutine != null)
            return;

        ExitAttackStateCoroutine = Coroutines.StartRoutine(Utilities.Utilities.DoMethodAftedDelay(ExitAttackStateFunc, ExitAttackStateDelay));

    }
    private void ExitAttackState()
    {
        enemyStateMachine.ChangeState(enemy.stayState);
        ExitAttackStateCoroutine = null;
        timer = 0;
    }
    private void CountTimeToShoot()
    {
        timer += Time.deltaTime;
        if (timer > shooter.ShootCooldown) 
        {
            shooter.Attack();
            timer = 0;
        }
    }
}
