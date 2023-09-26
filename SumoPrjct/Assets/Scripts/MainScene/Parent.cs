using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour
{
    [SerializeField]private float rotatingSpeed;

    private void Update()
    {
/*        Rotate();*/
    }
    private void Rotate()
    {
        transform.Rotate(new Vector3(0, rotatingSpeed, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.parent = transform;
    }
    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }
}
