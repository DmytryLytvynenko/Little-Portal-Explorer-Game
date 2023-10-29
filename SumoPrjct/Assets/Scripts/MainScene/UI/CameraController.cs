using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 posOffset;

    private void FixedUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x + posOffset.x, playerTransform.position.y +  posOffset.y, playerTransform.position.z + posOffset.z);
    }
}
