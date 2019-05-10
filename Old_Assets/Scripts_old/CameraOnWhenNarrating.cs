using UnityEngine;
using System.Collections;

public class CameraOnWhenNarrating : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (storyController.Narrating && storyController.disableCam)
            GetComponent<Camera>().enabled = true;
        else
            GetComponent<Camera>().enabled = false;

        
    }
}
