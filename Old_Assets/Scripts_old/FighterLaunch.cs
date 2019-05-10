using UnityEngine;
using System.Collections;

public class FighterLaunch : MonoBehaviour {

    public int planes = 3;
    public GameObject plane;
    public bool launchOne = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(ScoreKeeper.playerAlive.transform);
        if (launchOne)
        {
            LaunchPlane();
            launchOne = false;
        }
	}


    public void LaunchPlane()
    {
        if (planes > 0)
        {
            Instantiate(plane, transform.position, transform.rotation);
            planes--;
        }
    }
}
