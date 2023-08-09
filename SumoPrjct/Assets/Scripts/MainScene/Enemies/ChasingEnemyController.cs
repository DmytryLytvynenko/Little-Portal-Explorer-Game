using UnityEngine;

public class ChasingEnemyController : Enemy
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Hero").GetComponent<Transform>();
    }
    void Update()
    {
        Move();
    }
    private void OnCollisionEnter(Collision collision)
    {
        GiveContactDamage(collision);
    }
}
