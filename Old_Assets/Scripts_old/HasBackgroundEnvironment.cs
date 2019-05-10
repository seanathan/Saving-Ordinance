using UnityEngine;
using System.Collections;

public class HasBackgroundEnvironment : MonoBehaviour {

	
	// Update is called once per frame
	void LateUpdate () {
        //  if (EnvironmentCamera.env.activeCam.gameObject == this.gameObject)
        //    EnvironmentCamera.env.CameraMatch();
        EnvironmentCamera.env.transform.rotation = transform.rotation;
    }
}
