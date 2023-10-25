using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAccelerator : MonoBehaviour
{
    [SerializeField] private float accelerationForce;
    private Rigidbody body;
    private void OnCollisionEnter(Collision collision)
    {
        body = collision.gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!body)
            return;
        float currentVelocityXY = (Mathf.Abs(body.velocity.x) + Mathf.Abs(body.velocity.z));
        if (currentVelocityXY < accelerationForce)
        {
            body.AddForce(transform.forward * accelerationForce, ForceMode.Impulse);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        body = null;
    }
}
