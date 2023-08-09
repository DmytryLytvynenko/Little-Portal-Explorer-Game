using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
{
    public float X1;
    public float X2;
    public float Z1;
    public float Z2;

    public GameObject platform;// Кого спавним
    public Transform player;// Чтобы не спавнились на голову игроку
    public float playerRadius;

    public float spawnSpeed;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= spawnSpeed)
        {
            Spawn();
            timer = 0;
        }
        else
            timer += Time.deltaTime;
    }
    private void Spawn()
    {
        int angle = Random.Range(1, 360);
        float x = Random.Range(X1, X2);
        float z = Random.Range(Z1, Z2);
        Vector3 position = new Vector3(x, -5f, z);
        if ((Mathf.Pow((player.transform.position.x - x), 2) + Mathf.Pow((player.transform.position.z - z), 2)) >= Mathf.Pow(playerRadius, 2))// Если расстояние до игрока больше чем playerRadius, то платформа создается
        {
            Instantiate(platform, position, Quaternion.Euler(0f, angle, 0f));
        }
    }
}
