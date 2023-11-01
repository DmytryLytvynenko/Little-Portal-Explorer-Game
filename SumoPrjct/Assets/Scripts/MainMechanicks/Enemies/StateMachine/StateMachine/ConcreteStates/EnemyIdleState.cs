using UnityEngine;
using System.Collections.Generic;

public class EnemyIdleState : EnemyState
{
    public Vector3 target;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        target = GetRandomPositionInCircle();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        CheckTargetDistanse();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.Rotate(enemy.GetRotationVectorIdle(target));
        enemy.Move();
    }

    private Vector3 GetRandomPositionInCircle()
    {
        Vector3 randomInsideCircle = (Vector3)Random.insideUnitSphere;
        randomInsideCircle.y = enemy.transform.position.y;
        return enemy.transform.position + randomInsideCircle * enemy.RandomMoovementRange;
    }
    public void CheckTargetDistanse() 
    {
        if ((enemy.transform.position - target).magnitude < 1f)
        {
            target = GetRandomPositionInCircle();
        }
    }
}
