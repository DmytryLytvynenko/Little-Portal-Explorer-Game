using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private ShooterController parentEnemy;
    private void Start()
    {
        parentEnemy = GetComponentInParent<ShooterController>();
        GetComponent<SphereCollider>().radius = parentEnemy.AttackDistnace;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            parentEnemy.PlayerEnteredAttackZone?.Invoke();
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            parentEnemy.PlayerExitedAttackZone?.Invoke();
        }
    }
}
