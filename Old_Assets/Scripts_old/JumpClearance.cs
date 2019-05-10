using UnityEngine;
using System.Collections;

public class JumpClearance : MonoBehaviour {
    public bool jumpObstructed = false;
    private float timerCheck = 0f;

    void OnTriggerStay(Collider other)
    {
        jumpObstructed = true;
        timerCheck = 1f;

        Debug.Log("stay" + other.name);
    }

    void OnTriggerExit(Collider other)
    {
        jumpObstructed = false;
        timerCheck = 0f;

        Debug.Log("exit" + other.name);
    }

    void OnTriggerEnter(Collider other)
    {
        jumpObstructed = true;
        timerCheck = 1f;
        Debug.Log("enter" + other.name);
    }
	
	void Update () {
        if (timerCheck > 0)
        {
            timerCheck -= Time.deltaTime;
        }
        else
            jumpObstructed = false;
	}
    
}
