using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform targetPoint;
    private Rigidbody bullet;
	private GameObject pointer;
	private GameObject _pointer;
	[SerializeField] private int damage;
	[SerializeField] private int damageMultiplier;//если пуля ударит врага, его здоровье уменьшиться так же сильно, как и у игрока

	// Shoot characteristics
	public float jumpHeight = 7;
	public float gravity = -9.81f;
	public bool debugPath;

	private void Start()
	{
		pointer = Resources.Load<GameObject>("Prefabs/Pointer");
		bullet = GetComponent<Rigidbody>();
		bullet.useGravity = false;
		DrawPointer();
		Launch();
	}
	private void Launch()
	{
		bullet.useGravity = true;
		bullet.velocity = CalculateLaunchData().initialVelocity;
	}
	LaunchData CalculateLaunchData()
	{
		if (targetPoint.position.y >= bullet.position.y)
		{
			jumpHeight = targetPoint.position.y + 1;
		}
		else
		{
			jumpHeight = bullet.position.y + 1;
		}

		float displacementY = targetPoint.position.y - bullet.position.y;
		Vector3 displacementXZ = new Vector3(targetPoint.position.x - bullet.position.x, 0, targetPoint.position.z - bullet.position.z);
		float time = Mathf.Sqrt(-2 * jumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - jumpHeight) / gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * jumpHeight);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}
	private void DrawPath()
	{
		LaunchData launchData = CalculateLaunchData();
		Vector3 previousDrawPoint = bullet.position;

		int resolution = 30;
		for (int i = 1; i <= resolution; i++)
		{
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = bullet.position + displacement;

			Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
	}
	private void DrawPointer()
    {
		Ray ray = new Ray(targetPoint.position, -Vector3.up*10);
		RaycastHit hit;
		Physics.Raycast(ray, out hit);
		Vector3 drawPoint = hit.point;
		_pointer = Instantiate(pointer, drawPoint, Quaternion.identity);
	}
	struct LaunchData
	{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData(Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}

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
