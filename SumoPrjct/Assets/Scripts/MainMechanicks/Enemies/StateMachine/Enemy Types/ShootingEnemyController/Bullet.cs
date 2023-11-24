using UnityEngine;
using Launch;

public class Bullet : MonoBehaviour
{
    public Transform targetPoint;

	[SerializeField] private int damage;
	[SerializeField] private GameObject pointer;
	[SerializeField] private LayerMask layerMask;

    private Rigidbody bullet;
	private GameObject _pointer;
	Launch.Launch launch;

	// Shoot characteristics
	public float trajectoryHeight = 1.5f;
	public bool debugPath;

	private void Start()
	{
		launch = new Launch.Launch();
		bullet = GetComponent<Rigidbody>();
		launch.InitiateLaunch(targetPoint, bullet, trajectoryHeight, Physics.gravity.y);
		DrawPointer();
	}
	private void DrawPointer()
    {
		Ray ray = new Ray(targetPoint.position, -Vector3.up*100);
		RaycastHit hit;
		Physics.Raycast(ray, out hit,100f, layerMask);
		Vector3 drawPoint = hit.point;
		_pointer = Instantiate(pointer, drawPoint, Quaternion.identity);
	}
	private void OnCollisionEnter(Collision collision)
	{
		Destroy(_pointer);
		Destroy(this.gameObject);

		HealthControll healthControll;
        if (collision.gameObject.TryGetComponent(out healthControll))
		{
			healthControll.ChangeHealth(damage, transform);
		}
	}
}
