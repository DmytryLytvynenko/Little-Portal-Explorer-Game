using Sirenix.OdinInspector;
using Sound_Player;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Title("Main Stats")]
    [SerializeField] private float attackForce;
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask attackLayers;

    [Title("Links")]
    [SerializeField] private Transform attackArea;
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;

    private float xThrowSize;
    private float yThrowSize;
    private float zThrowSize;
    Quaternion attackAreaRotation;

    private void Start()
    {
        xThrowSize = attackArea.localScale.x * transform.localScale.x;
        yThrowSize = attackArea.localScale.y * transform.localScale.y;
        zThrowSize = attackArea.localScale.z * transform.localScale.z;
        attackAreaRotation = attackArea.rotation;
    }
    public void InitiateAttack(float attackForce, int attackDamage)
    {
        soundEffectPlayer.PlaySound(SoundName.Attack);
        Collider[] overlappedColiders = Physics.OverlapBox(attackArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), attackAreaRotation, attackLayers);
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
        VisualizeBox.DisplayBox(attackArea.position, new Vector3(xThrowSize / 2, yThrowSize / 2, zThrowSize / 2), attackArea.rotation);
    }
}
