using Sound_Player;
using System;
using System.Collections;
using UnityEngine;

public enum RunnerAnimParametrs
{
    Running
}
public class RunnerController : Enemy
{
    // Main characteristics
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float runDistnace;
    [SerializeField] private float runTime;
    [SerializeField] private float beforeRunDelay;
    [SerializeField] private float afterRunDelay;
    [SerializeField] private SphereCollider attackCollider;
    public RunnerRunState atackState { get; set; }

    #region Events

    public Action PlayerEnteredAttackZone;
    public Action PlayerExitedAttackZone;
    public Action EnemyEndedRun;

    protected override void Awake()
    {
        target = GlobalData.PlayerInstance.transform;
        base.Awake();

        atackState = new RunnerRunState(this, stateMachine, attackCollider, runDistnace, target, animator);
    }
    #endregion
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();

        base.Start();
    }
    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public override void Move()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    public IEnumerator Run()
    {
        Vector3 runVector;
        float timer = 0;
        float tempRotationSpeed = rotationSpeed;
        animator.SetBool(EnemyAnimParameters.Walking.ToString(), false);

        yield return new WaitForSeconds(beforeRunDelay);

        animator.SetBool(RunnerAnimParametrs.Running.ToString(), true);

        soundEffectPlayer.PlaySound(SoundName.Acceleration);
        runVector = (target.position - transform.position).normalized;
        rotationSpeed -= rotationSpeed;
        while (timer < runTime)
        {
            if (rb.velocity.magnitude < maxRunSpeed)
            {
                rb.AddForce(runVector * runSpeed, ForceMode.Impulse); 
            }
            timer += Time.deltaTime;
            yield return null;
        }
        rotationSpeed = tempRotationSpeed;
        animator.SetBool(RunnerAnimParametrs.Running.ToString(), false);
        animator.SetBool(EnemyAnimParameters.Walking.ToString(), false);
        yield return new WaitForSeconds(afterRunDelay);
        animator.SetBool(EnemyAnimParameters.Walking.ToString(), true);
        EnemyEndedRun?.Invoke();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
