using UnityEngine;

public class EnemyStayState : EnemyState
{
    public Transform target;
    public EnemyStayState(Enemy enemy, EnemyStateMachine enemyStateMachine, Transform Target) : base(enemy, enemyStateMachine)
    {
        target = Target;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        enemy.Rotate(target);
    }

}
