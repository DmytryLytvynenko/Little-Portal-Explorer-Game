using UnityEngine;

public class EnemyStayState : EnemyState
{
    public Transform target;
    private Animator animator;
    public EnemyStayState(Enemy enemy, EnemyStateMachine enemyStateMachine, Transform Target, Animator _animator) : base(enemy, enemyStateMachine)
    {
        target = Target;
        animator = _animator;
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
    }

    public override void PhysicsUpdate()
    {
        /*enemy.Rotate(target);*/
    }

}
