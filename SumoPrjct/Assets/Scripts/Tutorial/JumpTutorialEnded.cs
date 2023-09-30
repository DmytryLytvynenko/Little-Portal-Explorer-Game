using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorialEnded : MonoBehaviour
{
    [SerializeField] private GameObject throwButton;
    [SerializeField] private GameObject trowTip;
    private bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            throwButton.SetActive(true);
            trowTip.SetActive(true);
        }
    }
}
