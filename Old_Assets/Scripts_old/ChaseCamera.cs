using UnityEngine;
using System.Collections;

public class ChaseCamera : MonoBehaviour {
	
	public GameObject target;
	
	void LateUpdate () 
	{
		transform.position = target.transform.position;
		Vector3 pMomentum = target.GetComponent<Rigidbody>().velocity.normalized;
		Quaternion chase = Quaternion.LookRotation(pMomentum);
		transform.rotation = chase;
	}
}