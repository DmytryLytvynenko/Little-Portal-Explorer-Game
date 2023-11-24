using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    #region Fields
    [SerializeField] private StartState startState;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform IdleMoovementArea;
    [SerializeField] protected IdleTargetTrigger idleTargetTrigger;
    [SerializeField] private SphereCollider chaseCollider;
    [SerializeField] protected int contactDamage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float rotationSpeed;
    public float AgrDistance;

    protected bool isGrounded;
    protected Rigidbody rb;

    private enum StartState
    {
        Idle,
        Stay,
        Chase
    }
    #endregion

    #region StateMachineVariables
    protected EnemyStateMachine stateMachine { get; set; }

    public EnemyIdleState idleState { get; set; }
    public EnemyStayState stayState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    #endregion

    #region Events

    public static Action<int, Transform> EnemyAndPlayerContacted;
    public Action IdleTargetReached;
    public Action PlayerEnteredChaseZone;
    public Action PlayerExitedChaseZone;
    public Action EnemyDied;

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
    protected virtual void OnDisable()
    {
        stateMachine.AllStatesOnDisable();
    }
    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine, IdleMoovementArea, idleTargetTrigger.transform);
        chaseState = new EnemyChaseState(this, stateMachine, chaseCollider, AgrDistance, target);
        stayState = new EnemyStayState(this, stateMachine, target);
    }
    protected virtual void Start()
    {
        switch (startState)
        {
            case StartState.Idle:
                stateMachine.Initiallize(idleState);
                break;
            case StartState.Stay:
                stateMachine.Initiallize(stayState);
                break;
            case StartState.Chase:
                stateMachine.Initiallize(chaseState);
                break;
            default:
                break;
        }
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
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    protected void Die()
    {
        EnemyDied?.Invoke();
        Destroy(this.gameObject);
    }

    #endregion

    #region Callbacks
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>())//magic word!!!
            EnemyAndPlayerContacted?.Invoke(contactDamage, transform);

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


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (idleState != null)
        {
            Gizmos.DrawWireSphere(idleTargetTrigger.transform.position, 0.1f);
        }  
        if (chaseState != null)
        {
            Gizmos.DrawWireSphere(transform.position, AgrDistance);
        }
    }
    #endregion
}
