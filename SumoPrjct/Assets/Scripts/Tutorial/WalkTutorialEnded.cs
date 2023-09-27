using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTutorialEnded : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    Animator anim;
    private bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            _animation.Play();// Открываем проход дальше
            isTriggered = true;
        }
    }
}
