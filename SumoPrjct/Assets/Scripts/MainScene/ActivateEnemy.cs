using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<RunningEnemyController>())
        {
            gameObject.GetComponent<RunningEnemyController>().enabled = true;
        }

        if (gameObject.GetComponent<ShootingEnemyController>())
        {
            gameObject.GetComponent<ShootingEnemyController>().enabled = true;
        }

        if (gameObject.GetComponent<EnemyBigBoss>())
        {
            gameObject.GetComponent<EnemyBigBoss>().enabled = true;
        }

        if (gameObject.GetComponent<ChasingEnemyController>())
        {
            gameObject.GetComponent<ChasingEnemyController>().enabled = true;
        }
    }
}
