using System.Collections;
using System;
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

    public Action<ThrowerBomb> ThrowersBombExploded;

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
    private IEnumerator PlayAnimation()//использу€ этот метод примерно в 10% траэктори€ полета шалит
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
        ThrowersBombExploded?.Invoke(this);
    }
    public override void InitiateThrow()
    {
        base.InitiateThrow();
        transform.rotation = Quaternion.identity;
        moveVector = target.position - thrower.position;
        Debug.DrawLine(thrower.position, target.position, Color.red, 10f);
        StartCoroutine(PlayAnimation());
    }
}
