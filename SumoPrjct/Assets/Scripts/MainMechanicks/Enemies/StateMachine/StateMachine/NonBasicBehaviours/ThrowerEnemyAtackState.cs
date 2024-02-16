using ObjectsPool;
using System;
using UnityEngine;

public class ThrowerEnemyAtackState : EnemyState
{
    private Transform target;
    private ThrowerController thrower;
    private Action ExitAttackStateFunc;
    private Coroutine ExitAttackStateCoroutine;
    private float ExitAttackStateDelay = 2f;
    private float attackCooldown;
    private float timer = 0;
    private Animator animator;

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;

    #endregion

    public ThrowerEnemyAtackState(ThrowerController enemy, EnemyStateMachine enemyStateMachine, float AttackCooldown, Transform Target, Animator _animator) : base(enemy, enemyStateMachine)
    {
        target = Target;
        thrower = enemy;
        attackCooldown = AttackCooldown;
        ExitAttackStateFunc = ExitAttackState;
        animator = _animator;
    }

    public override void OnEnable()
    {
        thrower.PlayerEnteredAttackZone += OnPlayerEnteredAttackZone;
        thrower.PlayerExitedAttackZone += OnPlayerExitedAttackZone;
    }

    public override void OnDisable()
    {
        thrower.PlayerEnteredAttackZone -= OnPlayerEnteredAttackZone;
        thrower.PlayerExitedAttackZone -= OnPlayerExitedAttackZone;
    }
    public override void FrameUpdate()
    {
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
        if (timer > attackCooldown) 
        {
            thrower.MakeAttack();
            timer = 0;
        }
    }

}