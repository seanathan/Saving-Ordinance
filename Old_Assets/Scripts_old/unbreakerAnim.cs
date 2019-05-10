using UnityEngine;
using System.Collections;

public class unbreakerAnim : MonoBehaviour {
	public float startIn = 3.0f;
	public float timeSwitch;

	// Use this for initialization
	void Start () {
		timeSwitch = startIn + Time.time;

	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time > timeSwitch)
		{
			if (gameObject.GetComponent<Animator>().enabled == false)
			{
			gameObject.GetComponent<Animator>().enabled = true;
			}
			else if (gameObject.GetComponent<Animator>().enabled == true)
			{
				gameObject.GetComponent<Animator>().enabled = false;
			}
			timeSwitch = startIn + Time.time;

		}
	}
}
