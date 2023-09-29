using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotate : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float rotationSensitivity;
    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;
    public bool IsWorking { get; private set; }
    public void OnDrag(PointerEventData ped)
    {
        IsWorking = true;
        float xRotation = cameraRoot.localEulerAngles.x + ped.delta.y * rotationSensitivity;
        float yRotation = cameraRoot.localEulerAngles.y + ped.delta.x * rotationSensitivity;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        cameraRoot.localEulerAngles = new Vector3(xRotation, yRotation, 0);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        IsWorking = false;
    }
}
