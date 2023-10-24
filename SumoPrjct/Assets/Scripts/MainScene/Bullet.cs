using UnityEngine;
using Launch;

public class Bullet : MonoBehaviour
{
    public Transform targetPoint;

	[SerializeField] private int damage;
	[SerializeField] private int damageMultiplier;//если пуля ударит врага, его здоровье уменьшиться так же сильно, как и у игрока
	[SerializeField] private GameObject pointer;

    private Rigidbody bullet;
	private GameObject _pointer;
	Launch.Launch launch;

	// Shoot characteristics
	public float trajectoryHeight = 1.5f;
	public bool debugPath;

	private void Start()
	{
		launch = new Launch.Launch();
		launch.InitiateLaunch(targetPoint, bullet, trajectoryHeight, Physics.gravity.y);
		bullet = GetComponent<Rigidbody>();
		bullet.useGravity = false;
		DrawPointer();
	}
	private void DrawPointer()
    {
		Ray ray = new Ray(targetPoint.position, -Vector3.up*10);
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		Vector3 drawPoint = hit.point;
		_pointer = Instantiate(pointer, drawPoint, Quaternion.identity);
	}
	private void OnCollisionEnter(Collision collision)
	{
		Destroy(_pointer);
		Destroy(this.gameObject);
		if (!collision.gameObject.GetComponent<HealthControll>())
		{
			return;
		}
		else
		{
            if (!collision.gameObject.CompareTag("Player"))
            {
				damage *= damageMultiplier;
			}
			collision.gameObject.GetComponent<HealthControll>().ChangeHealth(-damage);
		}
	}
}
