using UnityEngine;
using System;

public class FrogController : Enemy
{
    const string ANIMATOR_PARAMETER_JUMP = "Jump";
    const string ANIMATOR_PARAMETER_DISABLED = "Disabled";

    [SerializeField] private float jumpForce;
    [Range(0f, 4f)]
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private Animator animator;

    [SerializeField] private Material[] materials;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    protected override void Awake()
    {
        IdleMoovementArea = GameObject.Find("FrogMoveArea").transform;
        base.Awake();
    }

    protected override void Start()
    {
        target = transform.GetChild(0).transform;
        target.parent = null;
        rb = GetComponent<Rigidbody>();
        PickRandomMaterial();


        base.Start();
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public override void Move()
    {
        rb.MovePosition(transform.position + GetRotationVector(target).normalized * Time.deltaTime * moveSpeed * Convert.ToInt32(isGrounded));
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        animator.SetBool(ANIMATOR_PARAMETER_JUMP, !isGrounded);
    }
    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        animator.SetBool(ANIMATOR_PARAMETER_JUMP, !isGrounded);
    }
    public void Disable()
    {
        animator.SetBool(ANIMATOR_PARAMETER_DISABLED, true);
        this.enabled = false;
    }
    private void PickRandomMaterial()
    {
        skinnedMeshRenderer.material = materials[UnityEngine.Random.Range(0, materials.Length)];
    }
    public void SetMoveArea(Transform moveArea)
    {
        this.IdleMoovementArea = moveArea;
    }
}