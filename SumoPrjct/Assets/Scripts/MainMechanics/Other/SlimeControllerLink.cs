using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeControllerLink : MonoBehaviour
{
    //Its for using an animation event on Slime
    [SerializeField] private ChasingEnemyController chasingEnemyController;

    public void InvokeDeath()
    {
        chasingEnemyController.EnemyDie();
    }
}
