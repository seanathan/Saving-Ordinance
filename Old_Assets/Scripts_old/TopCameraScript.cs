using UnityEngine;
using System.Collections;

public class TopCameraScript : MonoBehaviour {


	public Vector3 scalar;
	public Vector3 camSize;

	public GameObject target;
	public GameObject player;
	public float wheelPos = 0.0f;

	
	// Update is called once per frame
	void FixedUpdate () {
		float mouseW = Input.mouseScrollDelta.y;
		wheelPos = wheelPos - mouseW;
		
		//max wheel
		if (wheelPos > 9) 
		{
			wheelPos = 9;
		}
		//min wheel
		if (wheelPos < -9) 
		{
			wheelPos = -9;
		}
		
		//zoom alter
		camSize = scalar * (1 + (0.1f * wheelPos));
		
		
		transform.localScale = camSize;
	}

	void LateUpdate ()
	{
		transform.position = target.transform.position;
	}
}
