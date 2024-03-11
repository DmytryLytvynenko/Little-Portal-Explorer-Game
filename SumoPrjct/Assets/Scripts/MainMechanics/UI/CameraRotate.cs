using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float rotationLerpRate;

    private Quaternion targetRotation;
    public float xRotation { private get; set; }
    public float yRotation { private get; set; }
    private void LateUpdate()
    {
        float _xRotation = cameraRoot.localEulerAngles.x + xRotation * rotationSensitivity * Time.deltaTime;
        float _yRotation = cameraRoot.localEulerAngles.y + yRotation * rotationSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, minVerticalAngle, maxVerticalAngle);
        targetRotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        float t = Mathf.Clamp(Time.deltaTime * rotationLerpRate, 0f, 0.99f);
        cameraRoot.rotation = Quaternion.Lerp(cameraRoot.rotation, targetRotation, t);
        cameraRoot.rotation = Quaternion.Euler(cameraRoot.localEulerAngles.x, cameraRoot.localEulerAngles.y, 0f);

        //cameraRoot.rotation = targetRotation;

    }
}
