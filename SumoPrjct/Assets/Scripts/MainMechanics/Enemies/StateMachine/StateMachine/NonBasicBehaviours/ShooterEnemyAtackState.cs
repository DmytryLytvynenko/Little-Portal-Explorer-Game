using System;
using UnityEngine;

public class ShooterEnemyAtackState : EnemyState
{
    private Transform target;
    private ShooterController shooter;
    private Action ExitAttackStateFunc;
    private Coroutine ExitAttackStateCoroutine;
    private float ExitAttackStateDelay = 2f;
    private float shootCooldown;
    private float timer = 0;
    private Animator animator;

    public ShooterEnemyAtackState(ShooterController enemy, EnemyStateMachine enemyStateMachine,SphereCollider AttackCollider,float ShootCooldown,float AttackDistnace, Transform Target, Animator _animator) : base(enemy, enemyStateMachine)
    {
        target = Target;
        AttackCollider.radius = AttackDistnace;
        shooter = enemy;
        shootCooldown = ShootCooldown;
        timer = shootCooldown;
        ExitAttackStateFunc = ExitAttackState;
        animator = _animator;
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
        animator.SetBool(EnemyAnimParameters.Walking.ToString(), false);
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
            animator.SetTrigger(ShooterAnimParametrs.Attack.ToString());
/*            shooter.Attack();*/
            timer = 0;
        }
    }
}
