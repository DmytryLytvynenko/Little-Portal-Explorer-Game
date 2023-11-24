using UnityEngine;

public class BomberAttackTrigger : MonoBehaviour
{
    private BomberController parentEnemy;
    private void Start()
    {
        parentEnemy = GetComponentInParent<BomberController>();
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
