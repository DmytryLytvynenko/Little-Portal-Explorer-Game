using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHatch : MonoBehaviour
{
    [SerializeField] private bool isTrap;
    private Rigidbody body;
    private Quaternion defaultRotation;
    private Vector3 defaultposition;


    private void OnEnable()
    {
        Respawn.PlayerFell += ResetHatch;
    }
    private void OnDisable()
    {
        Respawn.PlayerFell -= ResetHatch;
    }
    void Start()
    {
        defaultposition = transform.position;
        defaultRotation = transform.rotation;
        body = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isTrap)
        {
            body.isKinematic = false;
        }
    }
    private void ResetHatch()
    {
        transform.position = defaultposition;
        transform.rotation = defaultRotation;
        body.isKinematic = true;
    }
}
