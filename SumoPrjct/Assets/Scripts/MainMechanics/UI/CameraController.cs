using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 posOffset;
    [SerializeField] private float positionLerpRate;
    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            playerTransform.position.x + posOffset.x, 
            playerTransform.position.y + posOffset.y, 
            playerTransform.position.z + posOffset.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionLerpRate);
    }
}
