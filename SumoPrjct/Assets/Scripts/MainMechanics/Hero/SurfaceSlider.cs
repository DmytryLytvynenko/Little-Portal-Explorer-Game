using System;
using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    [SerializeField] float acceptableVerticalAngle;
    [SerializeField] LayerMask collisionLayersToIgnore;
    private Vector3 normal;
    private Vector3 horizontal = Vector3.forward;
    private bool canMoveAtAngle;
    public Vector3 Project( Vector3 moveVector)
    {
        canMoveAtAngle = Mathf.Abs(Vector3.Angle(normal, transform.forward) - 90) <= acceptableVerticalAngle;
        return (moveVector - Vector3.Dot(moveVector, normal) * normal) * Convert.ToInt32(canMoveAtAngle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        normal = transform.up;
    }
    private void OnCollisionStay(Collision collision)
    {
        if ((collisionLayersToIgnore & (1 << collision.contacts[0].otherCollider.gameObject.layer)) != 0)
            return;

        normal = collision.contacts[0].normal;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + normal * 10);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward) );
    }
}