using System;
using System.Collections;
using System.Data;
using UnityEngine;
using Utilities;

public class EnemyChaseState : EnemyState
{
    public Transform target;
    private Action ExitChaseStateFunc;
    private float ExitChaseStateDelay = 2f; 
    private Coroutine ExitChaseStateCoroutine;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        ExitChaseStateFunc = ExitChaseState;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
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
        base.EnterState();
        target = enemy.Target.transform;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.Rotate(target);
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

        ExitChaseStateCoroutine = Coroutines.StartRoutine(DoMethodAftedDelay(ExitChaseStateFunc, ExitChaseStateDelay));
        
    }
    private void ExitChaseState()
    {
        enemyStateMachine.ChangeState(enemy.stayState);
        ExitChaseStateCoroutine = null;
    }
    private IEnumerator DoMethodAftedDelay(Action WaitForStateExit, float delay)
    {
        yield return new WaitForSeconds(delay);
        WaitForStateExit.Invoke();
    }
}