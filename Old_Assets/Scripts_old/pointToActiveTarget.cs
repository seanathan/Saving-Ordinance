using UnityEngine;
using System.Collections;

public class pointToActiveTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        if (DialogueBox.tracking != null)
        {
            transform.LookAt(DialogueBox.tracking.transform);
            GetComponentInChildren<Renderer>().enabled = true;
        }
        else
            GetComponentInChildren<Renderer>().enabled = false;
    }
}
