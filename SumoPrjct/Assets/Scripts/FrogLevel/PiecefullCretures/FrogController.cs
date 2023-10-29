using UnityEngine;
using System;

public class FrogController : Entity
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
    void Start()
    {
        target = transform.GetChild(0).transform;
        target.parent = null;
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
    void Update()
    {
        ChooseAnitherTarget();
        Rotate();
        Move();
    }
    protected override void Move()
    {
        rb.MovePosition(transform.position + rotationVector.normalized * Time.deltaTime * moveSpeed * Convert.ToInt32(isGrounded));
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
        target.position = randomPointInArea;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
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
