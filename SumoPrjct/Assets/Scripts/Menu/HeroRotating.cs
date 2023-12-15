using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRotating : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(rotationSpeed, rotationSpeed * 0.5f, rotationSpeed ) * Time.deltaTime);
    }
}
