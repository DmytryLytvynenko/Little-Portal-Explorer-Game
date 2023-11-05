using UnityEngine;

public class RunnerAttackTrigger : MonoBehaviour
{
    private RunnerController parentEnemy;
    private void Start()
    {
        parentEnemy = GetComponentInParent<RunnerController>();
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
