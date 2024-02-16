using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Transform target;
    private Transform moveArea;
    private LayerMask IdleLayers;
    private Animator animator;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine,Transform IdleMoovementArea, Transform IdleTargetTrigger,LayerMask _IdleLayers, Animator _animator) : base(enemy, enemyStateMachine)
    {
        moveArea = IdleMoovementArea;
        target = IdleTargetTrigger;
        IdleLayers = _IdleLayers;
        animator = _animator;
    }

    public override void OnEnable() { }

    public override void OnDisable(){ }

    public override void EnterState()
    {
        enemy.IdleTargetReached += OnIdleTargetReached;
        target.parent = null;
        target.gameObject.SetActive(true);
        target.position = GetRandomPositionInArea();
        animator.SetBool(EnemyAnimParameters.Walking.ToString(), true);
    }

    public override void ExitState()
    {
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
        Physics.Raycast(randomInsideBox, -Vector3.up,out hitInfo ,100,IdleLayers);
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
