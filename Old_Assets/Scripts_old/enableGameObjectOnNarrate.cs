using UnityEngine;
using System.Collections;

public class enableGameObjectOnNarrate : MonoBehaviour {
    public GameObject[] onNarrate;
    public GameObject[] offNarrate;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (storyController.Narrating && storyController.disableCam)
            foreach (GameObject thing in onNarrate)
                thing.SetActive(true);
        else
            foreach (GameObject thing in onNarrate)
                thing.SetActive(false);

        if (storyController.Narrating && storyController.disableCam)
            foreach (GameObject thing in offNarrate)
                thing.SetActive(false);
        else
            foreach (GameObject thing in offNarrate)
                thing.SetActive(true);
    }
}
