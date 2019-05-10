using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NarrationDisablesCanvas : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

        if (transform.GetComponentInChildren<Canvas>() != null)
            GetComponentInChildren<Canvas>().enabled = !storyController.Narrating;
    }
}
