using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    [SerializeField] private EnemySpawn Spawner; 
    [SerializeField] private AnimationCurve spawnRate; 
    [SerializeField] private float gameTimeInSeconds; 
    [SerializeField] private float difficultyIncreasesCount;
    [SerializeField] private Vector3 BossSpawnPoint;
    [SerializeField] private GameObject WinScreen;

    private GameObject boss;
    private bool bossSpawned = false;
    void Start()
    {
        Time.timeScale = 1;
        Spawner = gameObject.GetComponent<EnemySpawn>();
        StartCoroutine(SpawnScenario());
    }

    void Update()
    {
        if (!bossSpawned)
        {
            return;
        }
        if (boss == null)
        {
            WinScreen.SetActive(true);
            GameObject.Find("Hero").GetComponent<HeroController>().Win();
        }
    }
    private IEnumerator SpawnScenario()
    {
        float coefficient = gameTimeInSeconds / spawnRate.keys[spawnRate.keys.Length - 1].time;// подгоняем время gameTimeInSeconds под длину AnimationCurve
        for (int i = 0; i < difficultyIncreasesCount + 1; i++)
        {
            Spawner.spawnSpeed = spawnRate.Evaluate((gameTimeInSeconds / difficultyIncreasesCount * i) / coefficient);
            Debug.Log(Spawner.spawnSpeed);
            yield return new WaitForSeconds(gameTimeInSeconds / difficultyIncreasesCount);
        }
        Spawner.spawnSpeed = spawnRate.Evaluate(0);
        Debug.Log(Spawner.spawnSpeed);
        boss = Instantiate(Spawner.enemies[Spawner.enemies.Length - 1], BossSpawnPoint, Quaternion.identity);
        bossSpawned = true;
    }
}
