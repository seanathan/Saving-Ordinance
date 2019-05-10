using UnityEngine;
using System.Collections;

public class deckLanding : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		
		{
			other.GetComponent<Rigidbody> ().useGravity = true;
		}

	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 

		{
			other.GetComponent<Rigidbody> ().useGravity = false;
		}

	}
}
