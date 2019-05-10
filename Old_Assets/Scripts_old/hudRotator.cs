using UnityEngine;
using System.Collections;

public class hudRotator : MonoBehaviour 
{

	public float spin;

	// Use this for initialization
	void Start () 
	{
		// transform.Rotate (Vector3.up, spin * Time.deltaTime);
		GetComponent<Rigidbody>().transform.Rotate (Vector3.up, spin * Time.deltaTime);
	}

}
