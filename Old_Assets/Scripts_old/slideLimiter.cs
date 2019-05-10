using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class slideLimiter : MonoBehaviour {
	public int limitRange = 2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Mathf.Abs (GetComponent<Slider> ().value) < limitRange) 
		
		{
			(GetComponent<Slider> ().value) = 0;
		}
	}
}
