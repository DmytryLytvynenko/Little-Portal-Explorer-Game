using System.Collections;
using UnityEngine;

public class ThrowerDisk : Projectile
{
    [SerializeField] private float targetOffset;
    [SerializeField] private int damage = 20;
    [SerializeField] private float lerpRate = 10;

    private Vector3 actualTarget;
    private Vector3 startPosition;
    private float progress = 0;

    private void Awake()
    {
        target = GlobalData.PlayerInstance.transform;
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
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, lerpRate * Time.deltaTime);
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
        HealthControll helthControll;
        if (collision.gameObject.TryGetComponent<HealthControll>(out helthControll))
        {
            helthControll.ChangeHealth(-damage);
        }
    }
    public override void InitiateThrow()
    {
        base.InitiateThrow();
        transform.LookAt(target);
        startPosition = transform.position;
        moveVector = target.position - startPosition;
        actualTarget = transform.position + moveVector + moveVector.normalized * targetOffset;
        StartCoroutine(PlayAnimation());
    }
/*    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.DrawSphere(target.position, 0.1f);
    }
*/}
