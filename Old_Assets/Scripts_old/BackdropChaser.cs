using UnityEngine;
using System.Collections;

public class BackdropChaser : MonoBehaviour {
    public GameObject followObject;
    public float followSpeed = 10.0f;
    public bool followPlayer = false;

    void Start()
    {
        if (followPlayer)
        {
            followObject = ScoreKeeper.playerAlive;
        }
    }

	void Update () {
        transform.position = Vector3.Lerp(transform.position, followObject.transform.position, followSpeed * Time.deltaTime);
	}
}
