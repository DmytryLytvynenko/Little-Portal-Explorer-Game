using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawn : MonoBehaviour
{
    //Область в которой спавнятся враги: квадрат Х1, Х2, Z1, Z2
    public float X1;
    public float X2;
    public float Z1;
    public float Z2;

    public GameObject Bomb;
    public Transform player;// Чтобы не спавнились на голову игроку
    public float playerRadius;
    public float spawnSpeed;
    public float spawnHeight;
    private float timer;

    void Update()
    {
        if (timer >= spawnSpeed)
        {
            SpawnInArea();
            timer = 0;
        }
        else
            timer += Time.deltaTime;
    }

    private void SpawnInArea()
    {
        float x = Random.Range(X1, X2);
        float z = Random.Range(Z1, Z2);
        Vector3 position = new Vector3(x, spawnHeight, z);
        if ((Mathf.Pow((player.transform.position.x - x), 2) + Mathf.Pow((player.transform.position.z - z), 2)) >= Mathf.Pow(playerRadius, 2))// Если расстояние до игрока меньше чем playerRadius, то враг создается
        {
            Instantiate(Bomb, position, Quaternion.identity);
        }
    }
}
