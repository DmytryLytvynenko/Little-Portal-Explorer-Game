using System.Collections;
using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    [SerializeField] private int frogCount;
    [SerializeField] private GameObject frog;
    [SerializeField] private Transform frogMoveArea;
    [SerializeField] private float[] spawnTimeRange = new float[2];
    [SerializeField] private float verticalSpawnOffset;
    public int FrogCount { get { return frogCount; } private set { frogCount = value; } }
    private Vector3 randomPointInArea
    {
        get
        {
            float randomX = UnityEngine.Random.Range(-frogMoveArea.localScale.x / 2 + frogMoveArea.position.x, frogMoveArea.localScale.x / 2 + frogMoveArea.position.x);
            float randomZ = UnityEngine.Random.Range(-frogMoveArea.localScale.z / 2 + frogMoveArea.position.z, frogMoveArea.localScale.z / 2 + frogMoveArea.position.z);
            return new Vector3(randomX, frogMoveArea.position.y + verticalSpawnOffset, randomZ);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnFrogs());
    }

    private IEnumerator SpawnFrogs()
    {
        for (int i = 0; i < FrogCount; i++)
        {
            Instantiate(frog, randomPointInArea, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(spawnTimeRange[0], spawnTimeRange[1]));
        }
    }
}
