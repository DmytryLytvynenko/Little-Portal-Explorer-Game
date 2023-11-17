using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Launch;

public class JumperEnemy : Enemy
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [Range(0f, 4f)]
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float aimedJumpDistanse;
    Launch.Launch launch;

    private float timer = 0;

    protected override void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        launch = new Launch.Launch();
    }

    protected void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }
    protected void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public override void Move()
    {
        if (!JumpCooldownExpired())
            return;

        if (isGrounded)
        {
            if (GetRotationVector(target).magnitude < aimedJumpDistanse)
            {
                launch.InitiateLaunch(target, rb, jumpHeightMultiplier, Physics.gravity.y);
                isGrounded = false;
            }
            else
            {
                if (stateMachine.CurrentEnemyState == idleState)
                {
                    if (GetRotationVector(idleTargetTrigger.transform).magnitude < aimedJumpDistanse)
                    {
                        launch.InitiateLaunch(idleTargetTrigger.transform, rb, jumpHeightMultiplier, Physics.gravity.y);
                        isGrounded = false;
                    }                
                    else
                    {
                        rb.AddForce((transform.up * jumpHeightMultiplier + transform.forward).normalized * jumpForce, ForceMode.Impulse);
                        isGrounded = false;
                    }
                    return;
                }
                rb.AddForce((transform.up * jumpHeightMultiplier + transform.forward).normalized * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }
    private bool JumpCooldownExpired()
    {
        if (timer <= jumpCooldown)
        {
            timer += Time.deltaTime;
            return false;
        }

        timer = 0;
        return true;
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
