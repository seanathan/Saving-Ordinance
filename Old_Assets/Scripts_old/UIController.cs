using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
   // public bool overrideControl = false;
    public static bool suspendUI = false;

    void Start()
    {
        suspendUI = false;
    }
	// Update is called once per frame
	void Update () {

        
	}
    void LateUpdate()
    {
        GetComponent<Canvas>().enabled = !suspendUI;

       // if (!overrideControl)
       suspendUI = false;

        if (Pause.paused || storyController.Narrating)
        {
            suspendUI = true;
        }
    }
}