using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform thrower;
    [SerializeField] protected Animation projectileAnimation;
    [SerializeField] protected AnimationCurve throwTrajectory;
    [SerializeField] protected float duration;

    protected Vector3 moveVector;

    public virtual void InitiateThrow()
    {
        print("Throw Initiated");
    }
    public void SetThrower(Transform thrower)
    {
        this.thrower = thrower;
    }
}
