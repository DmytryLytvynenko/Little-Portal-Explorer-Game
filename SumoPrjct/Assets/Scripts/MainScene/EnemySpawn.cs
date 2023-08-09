using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //Область в которой спавнятся враги: квадрат Х1, Х2, Z1, Z2
    [SerializeField] private float X1;
    [SerializeField] private float X2;
    [SerializeField] private float Z1;
    [SerializeField] private float Z2;

    //окружность на которой спавнятся враги: круг с центром spawnAreaCenter и радиусом spawnAreaRadius
    [SerializeField] private Transform spawnAreaCenter;// центр окружности на которой спавнятся враги 
    [SerializeField] private float spawnAreaRadius;// радиус окружности на которой спавнятся враги 
    [SerializeField] private float spawnHeight;// высота на которой спавнится враг


    public GameObject[] enemies;// Кого спавним
    private GameObject enemy;
    [SerializeField] private Transform player;// Чтобы не спавнились на голову игроку
    [SerializeField] private float playerRadius;

    public float spawnSpeed;
    [SerializeField] private float pushForce;// сила толчка врага при создании
    private float timer;

    void Update()
    {
        if (timer >= spawnSpeed)
        {
            SpawnOnCircle();
            timer = 0;
        }
        else 
            timer += Time.deltaTime;
    }

    private void SpawnInArea()
    {
        ChooseEnemy();
        float x = Random.Range(X1, X2);
        float z = Random.Range(Z1, Z2);
        Vector3 position = new Vector3(x, 0.0f, z);
        if ( ( Mathf.Pow((player.transform.position.x - x), 2) + Mathf.Pow((player.transform.position.z - z), 2) ) >= Mathf.Pow(playerRadius, 2))// Если расстояние до игрока меньше чем playerRadius, то враг создается
        {
            Instantiate(enemy, position, Quaternion.identity);
        }
    }
    private void SpawnOnCircle()
    {
        ChooseEnemy();
        float x = Random.Range(0, spawnAreaRadius);
        float z = Mathf.Sqrt(Mathf.Pow(spawnAreaRadius, 2) - Mathf.Pow(x, 2)); // вычисление второой точки окружности по формуле пифагора

        int minus = Random.Range(0, 2);
        if (minus == 0)
        {
            z *= -1;
        }
        minus = Random.Range(0, 2);
        if (minus == 0)
        {
            x *= -1;
        }

        Vector3 position = new Vector3(x, spawnHeight, z);
        Vector3 pushDirection = new Vector3(spawnAreaCenter.position.x - x, 0.0f, spawnAreaCenter.position.z - z);
        GameObject _enemy = Instantiate(enemy, position, Quaternion.identity);
        pushEnemy(_enemy, pushDirection);
    }
    private void pushEnemy(GameObject enemy, Vector3 direction) 
    {
        enemy.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
    }
    private void ChooseEnemy()
    {
        int enemyNumber = Random.Range(0, enemies.Length - 1);
        enemy = enemies[enemyNumber];
    }
    private void SpawnBoss()
    {
        Vector3 position = new Vector3(0f, 10f, 5f);
        Instantiate(enemies[enemies.Length - 1], position, Quaternion.identity);
    }
}
