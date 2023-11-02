public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
        enemyStateMachine.AddState(this);
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType) { }
}
