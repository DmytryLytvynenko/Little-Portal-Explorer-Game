using UnityEngine;

public class EnemyStayState : EnemyState
{
    public Transform target;
    public EnemyStayState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
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
    }

    public override void PhysicsUpdate()
    {
        enemy.Rotate(target);
    }

}
