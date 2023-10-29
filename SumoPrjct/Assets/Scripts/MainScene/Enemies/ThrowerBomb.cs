using System.Collections;
using UnityEngine;

public class ThrowerBomb : Projectile
{
    [SerializeField] private float height;
    [SerializeField] private float pushForce;
    [SerializeField] private float explosionDelay;

    private Explosion explosion;
    private Rigidbody bombRigidbody;
    private float moveSpeed;
    private float startHeight;
    private float curveEnd;
    private int explosionDamage = 20;

    private void Awake()
    {
        target = GameObject.FindAnyObjectByType<HeroController>().transform;
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
        bombRigidbody.isKinematic = false;
        bombRigidbody.AddForce(moveVector.normalized * pushForce, ForceMode.Impulse);
        Invoke(nameof(Explode), explosionDelay);
    }
    private void MoveOnAnimation()
    {
        transform.Translate(moveVector * Time.deltaTime * moveSpeed);
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
        bombRigidbody.velocity = Vector3.zero;
        bombRigidbody.angularVelocity = Vector3.zero;
        moveVector = target.position - thrower.position;
        Debug.DrawLine(thrower.position, target.position, Color.red, 10f);
        StartCoroutine(PlayAnimation());
    }
}
