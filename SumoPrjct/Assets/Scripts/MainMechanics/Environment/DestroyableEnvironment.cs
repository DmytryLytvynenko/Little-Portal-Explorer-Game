using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEnvironment : MonoBehaviour
{
    [SerializeField] private float coinCount;
    [SerializeField] private float healCount;
    [SerializeField] private float dropFlyForce;
    private HealPool healPool;
    private CoinPool coinPool;
    [SerializeField] private HealthControll healthControll;

    private void OnEnable()
    {
        healthControll.EntityDied += OnEntityDied;
    }
    private void OnDisable()
    {
        healthControll.EntityDied -= OnEntityDied;
    }
    private void Start()
    {
        coinPool = GlobalData.coinPool;
        healPool = GlobalData.healPool;
    }
    private Vector3 RandomVector
    {
        get
        {
            return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }
    }
    private void DropReward()
    {
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = coinPool.GetCoin();
            coin.transform.position = transform.position;
            coin.GetComponent<Rigidbody>().AddForce((Vector3.up * 2 + RandomVector.normalized) * dropFlyForce);
        }
        for (int i = 0; i < healCount; i++)
        {
            GameObject heal = healPool.GetHeal();
            heal.transform.position = transform.position;
            heal.GetComponent<Rigidbody>().AddForce((Vector3.up * 2 + RandomVector.normalized) * dropFlyForce);
        }
        Destroy(gameObject);
    }
    private void OnEntityDied()
    {
        DropReward();
    }
}
