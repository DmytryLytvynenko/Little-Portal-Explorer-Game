using System;
using System.Collections;
using System.Data;
using UnityEngine;
using Utilities;

public class EnemyChaseState : EnemyState
{
    private Transform target;
    private Action ExitChaseStateFunc;
    private float ExitChaseStateDelay = 2f; 
    private Coroutine ExitChaseStateCoroutine;
    private SphereCollider chaseCollider;
    private Animator animator;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine, SphereCollider ChaseCollider, float AgrDistnace, Transform Target, Animator _animator) : base(enemy, enemyStateMachine)
    {
        target = Target;
        chaseCollider = ChaseCollider;
        chaseCollider.radius = AgrDistnace;
        ExitChaseStateFunc = ExitChaseState;
        animator = _animator;
    }
    public override void OnEnable()
    {
        enemy.PlayerEnteredChaseZone += OnPlayerEnteredChaseZone;
        enemy.PlayerExitedChaseZone += OnPlayerExitedChaseZone;
    }

    public override void OnDisable()
    {
        enemy.PlayerEnteredChaseZone -= OnPlayerEnteredChaseZone;
        enemy.PlayerExitedChaseZone -= OnPlayerExitedChaseZone;
    }
    public override void EnterState()
    {
        animator.SetBool(EnemyAnimParameters.Walking.ToString(), true);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.Rotate(target);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.Move();
    }
    private void OnPlayerEnteredChaseZone()
    {
        if (ExitChaseStateCoroutine != null)
        {
            Coroutines.StopRoutine(ExitChaseStateCoroutine);
            ExitChaseStateCoroutine = null;
            return;
        }
        if (enemyStateMachine.CurrentEnemyState == this)
            return;

        enemyStateMachine.ChangeState(this);
    }    
    private void OnPlayerExitedChaseZone()
    {
        if (ExitChaseStateCoroutine != null)
            return;

        ExitChaseStateCoroutine = Coroutines.StartRoutine(Utilities.Utilities.DoMethodAftedDelay(ExitChaseStateFunc, ExitChaseStateDelay));
        
    }
    private void ExitChaseState()
    {
        enemyStateMachine.ChangeState(enemy.idleState);
        ExitChaseStateCoroutine = null;
    }
}