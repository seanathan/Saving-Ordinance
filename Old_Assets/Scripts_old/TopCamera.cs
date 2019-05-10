using UnityEngine;
using System.Collections;

public class TopCamera : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject topCamera;
	public GameObject gunCamera;

	// Use this for initialization
	void Start () {
		mainCamera.SetActive (true);
		topCamera.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Back")) {
			mainCamera.SetActive (false);
			topCamera.SetActive (true);
			RenderSettings.fog = false;
			gunCamera.SetActive (false);
		} 
		else if (Input.GetButton ("Alt")) {
			mainCamera.SetActive (false);
			topCamera.SetActive (false);
			RenderSettings.fog = true;
			gunCamera.SetActive (true);
		}
		else
		{
			mainCamera.SetActive (true);
			topCamera.SetActive (false);
			RenderSettings.fog = true;
			gunCamera.SetActive (false);
		}
	}
}
