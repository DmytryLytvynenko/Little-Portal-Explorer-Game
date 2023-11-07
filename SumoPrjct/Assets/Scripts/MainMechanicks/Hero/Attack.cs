using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackForce;
    [SerializeField] private int attackDamage;
    [SerializeField] private Transform attackArea;

    private float xThrowSize;
    private float yThrowSize;
    private float zThrowSize;
    Quaternion attackAreaRotation;

    private void Start()
    {
        xThrowSize = attackArea.localScale.x; 
        yThrowSize = attackArea.localScale.y;
        zThrowSize = attackArea.localScale.z;
        attackAreaRotation = attackArea.rotation;
    }
    public void InitiateAttack(float attackForce, int attackDamage)
    {
        Collider[] overlappedColiders = Physics.OverlapBox(attackArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), attackAreaRotation);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (!rigitbody) continue;
            if (!overlappedColiders[i].gameObject.GetComponent<Enemy>()) continue;

            rigitbody.AddExplosionForce(attackForce, transform.position, attackArea.localScale.z);

            HealthControll healthControll;
            if (rigitbody.gameObject.TryGetComponent(out healthControll))
            {
                healthControll.ChangeHealth(-attackDamage);
            }
        }
    }
    public void AttackButton()
    {
        InitiateAttack(attackForce, attackDamage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))// потом убрать
        {
            AttackButton();
        }
    }
}
