using UnityEngine;
using System.Collections;

public class DropSmoke : MonoBehaviour {



	public GameObject smoke;
	public GameObject dropPoint;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Instantiate (smoke, dropPoint.transform.position, transform.rotation);
	}
}
