using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] enemySpawners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GlobalData.PlayerTag))
        {
            foreach (EnemySpawner spawner in enemySpawners)
            {
                spawner.StartSpawnCoroutine();
                gameObject.SetActive(false);
            }
        }
    }
}
