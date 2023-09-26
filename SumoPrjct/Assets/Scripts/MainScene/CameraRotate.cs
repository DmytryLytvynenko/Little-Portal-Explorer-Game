using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotate : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;
    public void OnDrag(PointerEventData ped)
    {
        float xRotation = cameraRoot.localEulerAngles.x + ped.delta.y * rotationSensitivity;
        float yRotation = cameraRoot.localEulerAngles.y + ped.delta.x * rotationSensitivity;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        cameraRoot.localEulerAngles = new Vector3(xRotation, yRotation, 0);
    }
}
