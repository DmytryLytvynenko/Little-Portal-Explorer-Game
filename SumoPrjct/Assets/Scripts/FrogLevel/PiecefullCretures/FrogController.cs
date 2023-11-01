using UnityEngine;
using System;

public class FrogController : Enemy
{
    const string ANIMATOR_PARAMETER_JUMP = "Jump";
    const string ANIMATOR_PARAMETER_DISABLED = "Disabled";

    [SerializeField] private float anotherTargetCooldown;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform moveArea;
    [Range(0f, 4f)]
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private Animator animator;

    [SerializeField] private Material[] materials;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private float timer = 0;
    protected override void Start()
    {
        stateMachine.ChangeState(chaseState);

        Target = transform.GetChild(0).transform;
        Target.parent = null;
        rb = GetComponent<Rigidbody>();

        PickRandomMaterial();
        ChangeTarget();
    }

    private Vector3 randomPointInArea 
    {
        get
        {
            float randomX = UnityEngine.Random.Range(-moveArea.localScale.x/2 + moveArea.position.x, moveArea.localScale.x/2 + moveArea.position.x);
            float randomZ = UnityEngine.Random.Range(-moveArea.localScale.z/2 + moveArea.position.z, moveArea.localScale.z/2 + moveArea.position.z);
            return new Vector3(randomX, 0, randomZ);
        }
    }
    protected override void Update()
    {
        ChooseAnitherTarget();
        Rotate(Target);
        Move();
    }
    public override void Move()
    {
        rb.MovePosition(transform.position + GetRotationVector(Target).normalized * Time.deltaTime * moveSpeed * Convert.ToInt32(isGrounded));
    }
    private void ChooseAnitherTarget()
    {
        timer += Time.deltaTime;
        if (timer > anotherTargetCooldown)
        {
            ChangeTarget();
            timer = 0;
        }
    }
    private void ChangeTarget()
    {
        Target.position = randomPointInArea;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Target.gameObject)
        {
            ChangeTarget();
            timer = 0;
        }
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
        this.moveArea = moveArea;
    }
}
