using System.Collections;
using UnityEngine;

public class ThrowerDisk : Projectile
{
    [SerializeField] private float distanceMultiplier;

    private Vector3 actualTarget;
    private Vector3 startPosition;
    private float progress = 0;
    private int damage = 20;

    private void Awake()
    {
        target = GameObject.FindAnyObjectByType<HeroController>().transform;
        projectileAnimation = GetComponentInChildren<Animation>();
    }
    private IEnumerator PlayAnimation()
    {
        float expiredTime = 0f;

        while (progress < 1)
        {
            expiredTime += Time.deltaTime;
            progress = expiredTime / duration;

            if (progress > 0.5f)
            {
                moveVector = actualTarget - thrower.position;
                transform.position = thrower.position + moveVector * throwTrajectory.Evaluate(progress);
            }
            else
            {
                moveVector = actualTarget - startPosition;
                transform.position = startPosition + moveVector * throwTrajectory.Evaluate(progress);
            }


            yield return null;
        }

        progress = 0;
        projectileAnimation.Stop();
        ProjectileDistroyed?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("HaveHealth"))
        {
            collision.gameObject.GetComponent<HealthControll>().ChangeHealth(-damage);
        }
    }
    public override void InitiateThrow()
    {
        base.InitiateThrow();
        startPosition = transform.position;
        moveVector = target.position - startPosition;
        actualTarget = transform.position + moveVector * distanceMultiplier;
        StartCoroutine(PlayAnimation());
    }
}
