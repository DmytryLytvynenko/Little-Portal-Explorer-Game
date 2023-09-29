using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTutorialEnded : MonoBehaviour
{
    [SerializeField] private Animation _passBlockerAnimation;
    [SerializeField] private GameObject jumpButton;
    [SerializeField] private GameObject jumpTip;
    Animator anim;
    private bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            _passBlockerAnimation.Play();// Открываем проход дальше
            isTriggered = true;
            jumpButton.SetActive(true);
            jumpTip.SetActive(true);
        }
    }
}
