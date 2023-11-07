using UnityEngine;

public class ThrowerAttackTrigger : MonoBehaviour
{
    private ThrowerController parentEnemy;
    private void Start()
    {
        parentEnemy = GetComponentInParent<ThrowerController>();
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
