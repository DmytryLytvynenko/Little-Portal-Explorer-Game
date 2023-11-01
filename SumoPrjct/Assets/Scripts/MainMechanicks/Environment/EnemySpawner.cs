using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int enemyCount;
    private IEnumerator StartSpawn()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            Enemy enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.SetTarget(GlobalData.PlayerInstance.transform);
            yield return new WaitForSeconds(spawnRate);
        }
    }
    public void StartSpawnCoroutine()
    {
        StartCoroutine(StartSpawn());
    }
}
