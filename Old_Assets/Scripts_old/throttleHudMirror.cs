using UnityEngine;
using System.Collections;

public class throttleHudMirror : MonoBehaviour {
    private GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        player = ScoreKeeper.playerAlive;

        if (player == null)
        {
            return;
        }
        //Input.GetAxis("Jump") >= 0.0f -- old



        transform.localScale = new Vector3(1f, player.GetComponent<PlayerControllerAlpha>().thrust / 10, 1f);

        /*
        if ((player.GetComponent<PlayerControllerAlpha>().thrust * 0.1f) >= 0.0f)
        {
            transform.localScale = Vector3.one;


        }

        else if ((player.GetComponent<PlayerControllerAlpha>().thrust * 0.1f) < 0.0f)
        {
            transform.localScale = new Vector3(1f, player.GetComponent<PlayerControllerAlpha>().thrust/10, 1f);
        }
        */
    }
}
