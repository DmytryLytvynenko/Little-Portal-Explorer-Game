using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject QuestTip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestTip.SetActive(true);
        }
    }
}
