using UnityEngine;
using System.Collections;

public class endCinematicAfterAnimation : MonoBehaviour {
    public float animationTime;
    private storyController story;
    public bool countdownToEnd = true;
    private bool ending = false;
	// Use this for initialization
	void Start () {
        story = GetComponentInParent<storyController>();

    }
	
	// Update is called once per frame
	void Update () {
        if (!ending)
        {
            if (storyController.Narrating && countdownToEnd)
            {
                animationTime -= Time.deltaTime;
            }
            if (animationTime < story.crossfader.fadeClock)
            {
                ending = true;
                story.EndCinematic();
            }
        }
	}
}
