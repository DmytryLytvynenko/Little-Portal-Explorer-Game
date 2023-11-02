using System.Data;
using UnityEngine;

delegate DataTable WaitForStateExit();
public class EnemyChaseState : EnemyState
{
    public Transform target;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }
    public override void OnEnable()
    {
        enemy.PlayerEnteredChaseZone += OnPlayerEnteredChaseZone;
    }

    public override void OnDisable()
    {
        enemy.PlayerEnteredChaseZone -= OnPlayerEnteredChaseZone;
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
        enemyStateMachine.ChangeState(this);
    }    
    private void OnPlayerExitedChaseZone()
    {

    }
    private void ExitChaseState()
    {
        enemyStateMachine.ChangeState(enemy.idleState);
    }
}