using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    #region Fields
    public Transform Target;

    public IdleTargetTrigger IdleTargetTrigger;
    public Transform IdleMoovementArea;

    [SerializeField] protected int contactDamage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float rotationSpeed;
    public float AgrDistance;

    protected bool isGrounded;
    protected Rigidbody rb;
    #endregion

    #region StateMachineVariables
    protected EnemyStateMachine stateMachine { get; set; }

    public EnemyIdleState idleState { get; set; }
    public EnemyStayState stayState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    #endregion

    #region Events

    public static Action<int> EnemyAndPlayerContacted;
    public Action IdleTargetReached;
    public Action PlayerEnteredChaseZone;
    public Action PlayerExitedChaseZone;

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

    #region MonoBehavior
    protected virtual void OnEnable()
    {
        stateMachine.AllStatesOnEnable();
    }
    private void OnDisable()
    {
        stateMachine.AllStatesOnDisable();
    }
    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        stayState = new EnemyStayState(this, stateMachine);
    }
    protected virtual void Start()
    {
        stateMachine.Initiallize(stayState);
    }
    #endregion

    #region Updates
    protected virtual void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    protected virtual void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #endregion

    #region Methods
    public Vector3 GetRotationVector(Transform target)// направление  передвижения
    {
        return new Vector3(target.position.x - transform.position.x, 0.0f, target.position.z - transform.position.z);
    }
    public virtual void Rotate(Transform target)
    {
        Vector3 rotationVector = GetRotationVector(target);
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
    public virtual void Attack() { }
    public void SetTarget(Transform target)
    {
        this.Target = target;
    }
    protected void Die()
    {
        Destroy(this.gameObject);
    }

    #endregion

    #region Callbacks
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (idleState != null)
        {
            Gizmos.DrawWireSphere(idleState.target.position, 0.1f);
        }  
        if (chaseState != null)
        {
            Gizmos.DrawWireSphere(transform.position, AgrDistance);
        }
    }
    #endregion
}
