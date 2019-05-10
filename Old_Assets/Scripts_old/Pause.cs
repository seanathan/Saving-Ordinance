using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	public static bool paused = false;

	// Use this for initialization
	void Start () {
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause"))
		{
			PauseButton();
		}
    }
	
	public static void PauseButton()
	{
        if (storyController.Narrating)
        {
            storyController.EndCutscene();
            return;

        }


        if (paused == false) 
		{
			paused = true;

			Time.timeScale = 0.0f;
		}
		else if (paused == true) 
		{
			paused = false;
			Time.timeScale = 1.0f;
		}
	}


}
