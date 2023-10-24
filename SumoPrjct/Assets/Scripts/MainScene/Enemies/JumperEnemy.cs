using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Launch;

public class JumperEnemy : Entity
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [Range(0f, 4f)]
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float aimedJumpDistanse;
    Launch.Launch launch;

    private float timer = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        launch = new Launch.Launch();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
    }

    protected override void Move()
    {
        if (!JumpCooldownExpired())
            return;

        if (isGrounded)
        {
            if (rotationVector.magnitude < aimedJumpDistanse)
            {
                launch.InitiateLaunch(target, rb, jumpHeightMultiplier, Physics.gravity.y);
                isGrounded = false;
            }
            else
            {
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
