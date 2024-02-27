using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : MonoBehaviour
{
    [SerializeField] private GameObject @object;

    public void HideObject()
    {
        @object.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        HideObject();
    }
}
