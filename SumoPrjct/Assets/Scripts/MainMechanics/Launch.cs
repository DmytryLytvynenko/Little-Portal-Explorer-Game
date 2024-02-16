using UnityEngine;

namespace Launch
{
	public class Launch 
    {
		public void InitiateLaunch(Transform targetPoint, Rigidbody launchObject, float jumpHeight, float gravity)
		{
			launchObject.useGravity = true;
			launchObject.velocity = CalculateLaunchData(targetPoint, launchObject, jumpHeight, gravity).initialVelocity;
		}
        public void InitiateLaunch(Vector3 targetPoint, Rigidbody launchObject, float jumpHeight, float gravity)
        {
            launchObject.useGravity = true;
            launchObject.velocity = CalculateLaunchData(targetPoint, launchObject, jumpHeight, gravity).initialVelocity;
        }
        LaunchData CalculateLaunchData(Transform targetPoint, Rigidbody launchObject, float jumpHeight, float gravity)
		{
			if (targetPoint.position.y >= launchObject.position.y)
			{
				jumpHeight = targetPoint.position.y - launchObject.position.y + jumpHeight;
			}

			float displacementY = targetPoint.position.y - launchObject.position.y;
			Vector3 displacementXZ = new Vector3(targetPoint.position.x - launchObject.position.x, 0, targetPoint.position.z - launchObject.position.z);
			float time = Mathf.Sqrt(-2 * jumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - jumpHeight) / gravity);
			Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * jumpHeight);
			Vector3 velocityXZ = displacementXZ / time;

			return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
		}
        LaunchData CalculateLaunchData(Vector3 targetPoint, Rigidbody launchObject, float jumpHeight, float gravity)
        {
            if (targetPoint.y >= launchObject.position.y)
            {
                jumpHeight = targetPoint.y - launchObject.position.y + jumpHeight;
            }

            float displacementY = targetPoint.y - launchObject.position.y;
            Vector3 displacementXZ = new Vector3(targetPoint.x - launchObject.position.x, 0, targetPoint.z - launchObject.position.z);
            float time = Mathf.Sqrt(-2 * jumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - jumpHeight) / gravity);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * jumpHeight);
            Vector3 velocityXZ = displacementXZ / time;

            return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }



        public void DrawPath(Transform targetPoint, Rigidbody launchObject, float jumpHeight, float gravity)
		{
			LaunchData launchData = CalculateLaunchData(targetPoint, launchObject, jumpHeight, gravity);
			Vector3 previousDrawPoint = launchObject.position;

			int resolution = 30;
			for (int i = 1; i <= resolution; i++)
			{
				float simulationTime = i / (float)resolution * launchData.timeToTarget;
				Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
				Vector3 drawPoint = launchObject.position + displacement;

				Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
				previousDrawPoint = drawPoint;
			}
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
	}
}
