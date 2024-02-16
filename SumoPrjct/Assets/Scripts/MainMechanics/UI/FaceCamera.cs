using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform.position);
        transform.Rotate(0, 180, 0);
    }
}
