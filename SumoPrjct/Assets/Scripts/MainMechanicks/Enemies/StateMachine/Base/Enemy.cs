using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    #region Fields
    public Transform Target;
    protected Rigidbody rb;
    [SerializeField] protected int contactDamage;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float rotationSpeed;
    protected bool isGrounded;
    #endregion

    #region StateMAchineVariables
    public EnemyStateMachine stateMachine { get; set; }

    public EnemyIdleState idleState { get; set; }
    public EnemyAtackState atackState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    #endregion

    #region IdleStateVariables
    [field: SerializeField] public float RandomMoovementRange { get; private set; } = 5f;
    [field: SerializeField] public float RandomMoovementSpeed { get; private set; } = 1f;
    #endregion

    #region Events
    public static Action<int> EnemyAndPlayerContacted;

    #endregion

    #region AnimationTriggers
    private void AnimationTriggerEvent(AnimationTriggerType animationTriggerType)
    {
        stateMachine.CurrentEnemyState.AnimationTriggerEvent(animationTriggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepsSound
    }

    #endregion

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        atackState = new EnemyAtackState(this, stateMachine);
    }

    protected virtual void Start()
    {
        stateMachine.Initiallize(idleState);
    }
    protected virtual void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    protected virtual void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public Vector3 GetRotationVectorChase()// направление  передвижения
    {
        return new Vector3(Target.position.x - transform.position.x, 0.0f, Target.position.z - transform.position.z);
    }
    public Vector3 GetRotationVectorIdle(Vector3 target)// направление  передвижения
    {
        return new Vector3(target.x - transform.position.x, 0.0f, target.z - transform.position.z);
    }
    public virtual void Rotate(Vector3 rotationVector)
    {
        if (rotationVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(rotationVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
    public virtual void Move()
    {
        //перемещение персонажа

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed * Convert.ToInt32(isGrounded), ForceMode.Impulse);//метод передвижения 
        }
    }
    public void SetTarget(Transform target)
    {
        this.Target = target;
    }  
    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))//magic word!!!
            EnemyAndPlayerContacted?.Invoke(contactDamage);

        if (collision.gameObject.CompareTag("Ground"))//magic word!!!
            isGrounded = true;
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))//magic word!!!
        {
            isGrounded = false;
        }
    }

    protected void Die()
    {
        Destroy(this.gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(idleState.target, 0.1f);
        Debug.Log($"{idleState.target}");
    }
}
