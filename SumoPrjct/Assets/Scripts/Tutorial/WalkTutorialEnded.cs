using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTutorialEnded : MonoBehaviour
{
    [SerializeField] private GameObject jumpTip;
    private bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            jumpTip.SetActive(true);
        }
    }
}
