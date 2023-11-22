using System.Collections;
using UnityEngine;

public class ThrowerBomb : Projectile
{
    [SerializeField] private float height;
    [SerializeField] private float pushForce;
    [SerializeField] private float explosionDelay;
    [SerializeField] private int explosionDamage = 20;

    private Explosion explosion;
    private Rigidbody bombRigidbody;
    private float moveSpeed;
    private float startHeight;
    private float curveEnd;

    private void Awake()
    {
        target = GlobalData.PlayerInstance.transform;
        projectileAnimation = GetComponentInChildren<Animation>();
        explosion = GetComponent<Explosion>();
        bombRigidbody = GetComponent<Rigidbody>();
        curveEnd = throwTrajectory.keys[throwTrajectory.length - 1].time;
        moveSpeed = 1 / (duration);
        startHeight = transform.position.y;
    }
    private IEnumerator PlayAnimation()
    {
        float expiredTime = 0f;
        float progress = 0f;

        while (progress < curveEnd)
        {
            expiredTime += Time.deltaTime;
            progress = expiredTime / duration;
            transform.position = new Vector3(transform.position.x, startHeight + throwTrajectory.Evaluate(progress) * height, transform.position.z);

            MoveOnAnimation();

            yield return null;
        }

        projectileAnimation.Stop();
        bombRigidbody.useGravity = true;
        bombRigidbody.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        Invoke(nameof(Explode), explosionDelay);
    }
    private void MoveOnAnimation()
    {
        transform.position += moveVector * Time.deltaTime * moveSpeed;
    }
    private void Explode()
    {
        explosion.Explode(explosionDamage);
        ProjectileDistroyed?.Invoke(this);
    }
    public override void InitiateThrow()
    {
        base.InitiateThrow();
        transform.rotation = Quaternion.identity;
        transform.LookAt(target.position);
        bombRigidbody.velocity = Vector3.zero;
        bombRigidbody.angularVelocity = Vector3.zero;
        moveVector = new Vector3(target.position.x - thrower.position.x,0, target.position.z - thrower.position.z);
        Debug.DrawLine(thrower.position, target.position, Color.red, 10f);
        StartCoroutine(PlayAnimation());
    }
}
