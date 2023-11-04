using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Transform target;
    private Transform moveArea;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine,Transform IdleMoovementArea, Transform IdleTargetTrigger) : base(enemy, enemyStateMachine)
    {
        moveArea = IdleMoovementArea;
        target = IdleTargetTrigger;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }
    public override void OnEnable() { }

    public override void OnDisable(){ }

    public override void EnterState()
    {
        enemy.IdleTargetReached += OnIdleTargetReached;
        target.parent = null;
        target.gameObject.SetActive(true);
        target.position = GetRandomPositionInArea();
    }

    public override void ExitState()
    {
        base.ExitState();
        target.gameObject.SetActive(false);
        enemy.IdleTargetReached -= OnIdleTargetReached;
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

    private Vector3 GetRandomPositionInArea()
    {
        int multiplier = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector3 randomInsideBox = new Vector3
            (
            moveArea.position.x + Random.Range(0, moveArea.localScale.x / 2) * multiplier,
            moveArea.transform.position.y,
            moveArea.position.z + Random.Range(0, moveArea.localScale.z / 2) * multiplier
            );

        RaycastHit hitInfo;
        Physics.Raycast(randomInsideBox, -Vector3.up,out hitInfo ,100);
        Debug.DrawRay(randomInsideBox, -Vector3.up, Color.red, 100);

        if (hitInfo.collider == null)
        {
            Debug.Log("Под зоной нет коллайдера");
            return Vector3.zero;
        }

        return hitInfo.point;
    }
    public void OnIdleTargetReached() 
    {
        target.position = GetRandomPositionInArea();
    }

}
