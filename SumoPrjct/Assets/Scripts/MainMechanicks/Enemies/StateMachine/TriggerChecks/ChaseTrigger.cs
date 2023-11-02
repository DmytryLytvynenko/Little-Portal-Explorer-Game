using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    private Enemy parentEnemy;
    private void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
        GetComponent<SphereCollider>().radius = parentEnemy.AgrDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            parentEnemy.PlayerEnteredChaseZone?.Invoke();
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            parentEnemy.PlayerExitedChaseZone?.Invoke();
        }
    }
}
