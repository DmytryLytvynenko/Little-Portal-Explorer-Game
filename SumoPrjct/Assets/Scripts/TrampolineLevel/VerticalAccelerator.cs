using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalAccelerator : MonoBehaviour
{
    [SerializeField] private float accelerationForce;
    private Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SpeedUp();
        }
    }
    public void SpeedUp()
    {
        body.AddForce(-transform.up * accelerationForce, ForceMode.Impulse); 
    }
}
