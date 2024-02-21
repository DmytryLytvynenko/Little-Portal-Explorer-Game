using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEnvironment : MonoBehaviour
{
    [SerializeField] protected float minProbability = 0;
    [SerializeField] protected float maxProbability = 1;
    [SerializeField] protected float coinCount;
    [SerializeField] protected float healCount;
    [SerializeField] protected float dropFlyForce;
    [SerializeField] protected float dropDelay;
    protected HealPool healPool;
    protected CoinPool coinPool;
    [SerializeField] protected HealthControll healthControll;
    [SerializeField] protected GameObject brakeEffect;

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
        coinCount = Mathf.Round(coinCount * Random.Range(minProbability, maxProbability));
        coinPool = GlobalData.coinPool;
        healPool = GlobalData.healPool;
    }
    protected Vector3 RandomVector
    {
        get
        {
            return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }
    }
    protected virtual IEnumerator DropReward()
    {
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(brakeEffect, transform.position, Quaternion.identity);
        for (int i = 0; i < coinCount; i++)
        {
            yield return new WaitForSeconds(dropDelay);
            GameObject coin = coinPool.GetCoin();
            coin.transform.position = transform.position;
            coin.GetComponent<Rigidbody>().AddForce((Vector3.up * 2 + RandomVector.normalized) * dropFlyForce);
        }
        for (int i = 0; i < healCount; i++)
        {
            yield return new WaitForSeconds(dropDelay);
            GameObject heal = healPool.GetHeal();
            heal.transform.position = transform.position;
            heal.GetComponent<Rigidbody>().AddForce((Vector3.up * 2 + RandomVector.normalized) * dropFlyForce);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    protected virtual void OnEntityDied()
    {
       StartCoroutine(nameof(DropReward));
    }
}
