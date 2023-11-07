using ObjectsPool;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject pool;
    private GameObjectPool _pool;

    private void Awake()
    {
        _pool = pool.GetComponent<GameObjectPool>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>()) 
        {
            _pool.Return(gameObject);
        }
    }
}
