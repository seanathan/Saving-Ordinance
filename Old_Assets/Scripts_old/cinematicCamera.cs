using UnityEngine;
using System.Collections;

public class cinematicCamera : MonoBehaviour {

	public GameObject subjectA;
	public GameObject subjectB;
	public GameObject gyroA;
	public GameObject gyroB;
	public GameObject gyroCam;
	public GameObject focus;

	public float angleA;
	public float angleB;
	public float camSpeed;
	public float chaseDistance;
	public float currentDistance;
	public float dollySpeed;

	void Start()
	{
		chaseDistance = Vector3.Distance (subjectA.transform.position, gyroCam.transform.position);
	
	}
	// Update is called once per frame
	void LateUpdate () {
		currentDistance = Vector3.Distance (subjectA.transform.position, gyroCam.transform.position);
		//set distance from subject A

		//aim for spot directly between the two positions
		focus.transform.position = subjectB.transform.position + ((subjectA.transform.position - subjectB.transform.position) / 2.0f);
		focus.transform.LookAt (gyroCam.transform);

		gyroA.transform.position = subjectA.transform.position;
		gyroA.transform.LookAt (gyroCam.transform);

		gyroB.transform.position = subjectB.transform.position;
		gyroB.transform.LookAt (gyroCam.transform);

		gyroCam.transform.LookAt (focus.transform);

		//angle with respect to camera
//		gyroB.transform.rotation.eulerAngles.Normalize
		angleA = Quaternion.Angle (gyroA.transform.rotation, focus.transform.rotation);
		angleB = Quaternion.Angle (gyroB.transform.rotation, focus.transform.rotation);




		//chase dolly
		if (currentDistance > (chaseDistance * 1.2)) 
		{
			gyroCam.transform.Translate(Vector3.forward * Time.deltaTime * (camSpeed + ScoreKeeper.playerSpeed));
		}

		else if (currentDistance < (chaseDistance * 0.8f)) 
		{
			gyroCam.transform.Translate(Vector3.back * Time.deltaTime * (camSpeed + ScoreKeeper.playerSpeed));
		}


		//framing dolly
		if (angleA > 45.0f) 
		{
			//dolly back	
			gyroCam.transform.Translate(Vector3.back * Time.deltaTime *  (camSpeed + ScoreKeeper.playerSpeed));

		}
		else if (angleA < 15.0f) 
		{
			//dolly back	
			gyroCam.transform.Translate(Vector3.forward * Time.deltaTime * (camSpeed + ScoreKeeper.playerSpeed));

		}

		//move camera to gyro
		Vector3 newPos = Vector3.Lerp(transform.position, gyroCam.transform.position, dollySpeed * Time.deltaTime);


		transform.position = newPos;

		//rotate camera to gyro
			Quaternion newRot = Quaternion.Lerp(transform.rotation, gyroCam.transform.rotation, dollySpeed * Time.deltaTime);

			transform.rotation = newRot;



	}


}
