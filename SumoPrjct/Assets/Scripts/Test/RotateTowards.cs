using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    [SerializeField] Transform target;
    void Start()
    {
        
    }
    void Update()
    {
        transform.LookAt(target);
    }
}
