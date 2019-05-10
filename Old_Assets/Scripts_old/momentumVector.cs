using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class momentumVector : MonoBehaviour {

	public GameObject player;
	public GameObject momX;
	public GameObject momY;
	public GameObject momZ;
	public float magnitudescale = 1.0f;
	public Text texts;

	void LateUpdate () 
	{
		transform.position = player.transform.position;
		Vector3 pVelocity = player.GetComponent<Rigidbody>().velocity.normalized;
	//	Vector3 pOrient = player.transform.rotation.eulerAngles.normalized;
		float pMagn = player.GetComponent<Rigidbody>().velocity.magnitude;
		Vector3 scalar = new Vector3(1,1,1);
		momZ.transform.localScale = scalar * pMagn * magnitudescale; //amount in forward direction
		//momY.transform.localScale = pVelocity.y * scalar * magnitudescale; //amount sideways
		//momZ.transform.localScale = pVelocity.z * scalar * magnitudescale; //amount elevating


		texts.text = "Velocity = " + pMagn.ToString ();
		Quaternion chase = Quaternion.LookRotation(pVelocity);
		transform.rotation = chase;
	}
}
