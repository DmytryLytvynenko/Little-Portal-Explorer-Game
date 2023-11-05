using System;
using UnityEngine;

public class RunnerRunState : EnemyState
{
    private Transform target;
    private RunnerController runner;
    private Coroutine runCoroutine;
    private float runDelay = 2f;
    private float timer = 0;
    private Zone currentZone;

    private enum Zone
    {
        attack,
        chase,
        none
    }

    public RunnerRunState(RunnerController enemy, EnemyStateMachine enemyStateMachine, SphereCollider AttackCollider,  float AttackDistnace, Transform Target) : base(enemy, enemyStateMachine)
    {
        target = Target;
        AttackCollider.radius =  AttackDistnace;
        runner = enemy;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }
    public override void OnEnable()
    {
        runner.PlayerEnteredAttackZone += OnPlayerEnteredAttackZone;
        runner.PlayerExitedAttackZone += OnPlayerExitedAttackZone;

        runner.PlayerExitedChaseZone += OnPlayerExitedChaseZone;

        runner.EnemyEndedRun += OnEnemyEndedRun;
    }

    public override void OnDisable()
    {
        runner.PlayerEnteredAttackZone -= OnPlayerEnteredAttackZone;
        runner.PlayerExitedAttackZone -= OnPlayerExitedAttackZone; 
        
        runner.PlayerExitedChaseZone -= OnPlayerExitedChaseZone;

        runner.EnemyEndedRun -= OnEnemyEndedRun;
    }
    public override void EnterState()
    {
        currentZone = Zone.attack;
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        enemy.Rotate(target);
    }

    private void OnPlayerEnteredAttackZone()
    {
        if (runCoroutine != null)
            return;


        enemyStateMachine.ChangeState(this);
        runCoroutine = Coroutines.StartRoutine(runner.Run(runDelay));
        Debug.Log(timer);
    }
    private void OnPlayerExitedAttackZone()
    {
        currentZone = Zone.chase;

    }
    private void OnPlayerExitedChaseZone()
    {
        currentZone = Zone.none;
    }
    private void OnEnemyEndedRun()
    {
       switch (currentZone) 
        {
            case Zone.none:
                enemyStateMachine.ChangeState(enemy.idleState);
                break;
            case Zone.chase:
                enemyStateMachine.ChangeState(enemy.chaseState);
                break;
            case Zone.attack: 
                OnPlayerEnteredAttackZone();
                break;

            default: break;
        }
        runCoroutine = null;
    }
}

