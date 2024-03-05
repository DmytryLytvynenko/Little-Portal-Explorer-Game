using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorialEnded : MonoBehaviour
{
    [SerializeField] private GameObject throwButton;
    [SerializeField] private GameObject throwTip;
    [SerializeField] private GameObject attackButton;
    private bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            throwButton.SetActive(true);
            throwTip.SetActive(true);
            attackButton.SetActive(true);
        }
    }
}
