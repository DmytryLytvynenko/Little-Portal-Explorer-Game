using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHatch : MonoBehaviour
{
    [SerializeField] private bool isTrap;
    private Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isTrap)
            body.isKinematic = false;
    }
}
