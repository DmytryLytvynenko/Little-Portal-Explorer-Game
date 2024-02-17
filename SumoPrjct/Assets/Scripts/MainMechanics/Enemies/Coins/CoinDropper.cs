using ObjectsPool;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    [SerializeField] private int coinCount;
    [SerializeField] private float coinFlyForce;
    [SerializeField] private Enemy enemy;
    [SerializeField] private CoinPool pool;
    private Vector3 RandomVector
    {
        get
        {
            return new Vector3(Random.Range(-10,10),0, Random.Range(-10, 10));
        }
    }

    private void OnEnable()
    {
        enemy.EnemyDied += OnEnemyDied;
    }
    private void OnDisable()
    {
        enemy.EnemyDied -= OnEnemyDied;
    }
    private void Start()
    {
        pool = GlobalData.coinPool;
    }
    private void DropCoin()
    {
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = pool.GetCoin();
            coin.transform.position = transform.position;
            coin.GetComponent<Rigidbody>().AddForce((transform.up + RandomVector.normalized) * coinFlyForce);
        }
    }
    private void OnEnemyDied()
    {
        DropCoin();
    }
}
