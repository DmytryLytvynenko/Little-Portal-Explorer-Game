using ObjectsPool;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    [SerializeField] private int coinCount;
    [SerializeField] private float coinFlyForce;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject pool;
    private GameObjectPool _pool;
    private void Awake()
    {
        _pool = pool.GetComponent<GameObjectPool>();
    }
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
    private void DropCoin()
    {
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = _pool.Get();
            coin.transform.position = transform.position;
            coin.GetComponent<Rigidbody>().AddForce((transform.up + RandomVector.normalized) * coinFlyForce);
        }
    }
    private void OnEnemyDied()
    {
        DropCoin();
    }
}
