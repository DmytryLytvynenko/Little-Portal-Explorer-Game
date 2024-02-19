using UnityEngine;

public class BomberBomb : Projectile
{
    [SerializeField] private float explosionDelay;
    [SerializeField] private int explosionDamage = 20;
    [SerializeField] private float throwHeight;
    [SerializeField] private float targetOffset;
    [SerializeField] private MeshRenderer bombRenderer;

    protected Explosion explosion;
    private Rigidbody bombRigidbody;
    private Launch.Launch launch;

    public void Awake()
    {
        launch = new Launch.Launch();
        target = GlobalData.PlayerInstance.transform;/*
        projectileAnimation = GetComponentInChildren<Animation>();*/
        explosion = GetComponent<Explosion>();
        bombRigidbody = GetComponent<Rigidbody>();
    }
    private void Explode()
    {
        explosion.Explode(explosionDamage);
        bombRenderer.enabled = false;
        Invoke(nameof(Die), 2);
    }
    public override void InitiateThrow()
    {
        base.InitiateThrow();

        transform.position = thrower.position;
        bombRigidbody.position = thrower.position;
        bombRigidbody.velocity = Vector3.zero;

        Vector3 moveVector = target.position - transform.position;
        Vector3 actualTarget = transform.position + moveVector - moveVector.normalized * targetOffset;
        Debug.DrawLine(target.position, transform.position, Color.green, 10f);
        Debug.DrawLine(thrower.position, actualTarget, Color.red, 10f);

        launch.InitiateLaunch(actualTarget, bombRigidbody, throwHeight, Physics.gravity.y);
        Invoke(nameof(Explode), explosionDelay);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
