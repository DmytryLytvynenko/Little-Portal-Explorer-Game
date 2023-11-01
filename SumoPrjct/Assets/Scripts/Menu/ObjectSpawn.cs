using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;// Кого спавним
    private GameObject spawnObject;

    [SerializeField] private float spawnSpeed;
    private float timer = 0;
    private void Start()
    {
        Time.timeScale = 1;
    }
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
        ChooseObject();
        int x = Random.Range(-7, 8);
        Vector3 position = new Vector3(x, transform.position.y, transform.position.z);
        Instantiate(spawnObject, position, Quaternion.identity);
    }
    private void ChooseObject()
    {
        int objectNumber = Random.Range(0, objects.Length);
        spawnObject = objects[objectNumber];
    }
}
