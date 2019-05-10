using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fpscount : MonoBehaviour {

    private float nextSec;
    public float fps;
    private int lastFrameCount;
    public float interval = 1;
	
	// Update is called once per frame
	void Update () {

        //update every 1 second
        fps = Time.frameCount - lastFrameCount;
        
        if (nextSec <= Time.time)
        {
           transform.GetComponent<Text>().text = (Mathf.RoundToInt(fps / interval)).ToString() + " FPS";
   
            nextSec = Time.time + interval;
            lastFrameCount = Time.frameCount;

        }
//		transform.GetComponent<Text>().text = Mathf.RoundToInt(1.0f/Time.deltaTime).ToString() + " FPS";
	}
    
}
