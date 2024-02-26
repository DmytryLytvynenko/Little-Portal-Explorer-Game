using UnityEngine;
using System;
using Sound_Player;

public enum EnemyAnimParameters
{
    Walking,
    Death,
    Hit
}
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    #region Fields
    [SerializeField] private StartState startState;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform IdleMoovementArea;
    [SerializeField] protected IdleTargetTrigger idleTargetTrigger;
    [SerializeField] private SphereCollider chaseCollider;
    [SerializeField] protected HealthControll healthControll;
    [SerializeField] protected SoundEffectPlayer soundEffectPlayer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected int contactDamage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected LayerMask IdleLayers;
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

    #region MonoBehavior
    protected virtual void OnEnable()
    {
        healthControll.EntityDied += OnEntityDied;
        healthControll.DamageTaken += OnDamageTaken;
        stateMachine.AllStatesOnEnable();
    }
    protected virtual void OnDisable()
    {
        healthControll.EntityDied -= OnEntityDied;
        healthControll.DamageTaken -= OnDamageTaken;
        stateMachine.AllStatesOnDisable();
    }
    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine, IdleMoovementArea, idleTargetTrigger.transform,IdleLayers, animator);
        chaseState = new EnemyChaseState(this, stateMachine, chaseCollider, AgrDistance, target, animator);
        stayState = new EnemyStayState(this, stateMachine, target, animator);
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
    public Vector3 GetRotationVector(Transform target)// �����������  ������������
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
        //����������� ���������

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed * Convert.ToInt32(isGrounded), ForceMode.Impulse);//����� ������������ 
        }
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public virtual void EnemyDie()
    {
        EnemyDied?.Invoke();
        Destroy(this.gameObject);
    }

    #endregion

    #region Callbacks
    private void OnEntityDied()
    {
        animator.SetBool(EnemyAnimParameters.Death.ToString(), true);
        this.enabled = false;
    }
    private void OnDamageTaken(Transform damager)
    {
        animator.SetTrigger(EnemyAnimParameters.Hit.ToString());
    }
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