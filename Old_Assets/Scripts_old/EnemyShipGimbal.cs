using UnityEngine;
using System.Collections;

public class EnemyShipGimbal : MonoBehaviour {

    [Header("Deprecated")]

	public float bankGimbalForce = -0.5f;
	private GameObject gyro;
    public float deltaY = 0.0f;
    public float deltaZ = 0.0f;

    public float resetTime = 3.0f;

	public float banker;



	void FixedUpdate() 
	{
        //angle between gyro and parent
        if (gyro == null)
        {
            if (GetComponentInParent<LameShip>() != null)
                gyro = GetComponentInParent<LameShip>().getNavGyro();

            else if (GetComponentInParent<EnemyShipModular>() != null)
                gyro = GetComponentInParent<EnemyShipModular>().navGyro;

        }
        if (gyro == null)
            return;


		deltaY = gyro.transform.localRotation.eulerAngles.y - transform.localRotation.eulerAngles.y;

		if (deltaY > 180) 
		{
			deltaY -= 360;
		}

        deltaZ = gyro.transform.localRotation.eulerAngles.z - transform.localRotation.eulerAngles.z;

        if (deltaZ > 180)
        {
            deltaZ -= 360;
        }

        banker = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, deltaY * bankGimbalForce, resetTime * Time.deltaTime);

		transform.localRotation = Quaternion.Euler (0.0f, 0.0f, banker);
	}

}
