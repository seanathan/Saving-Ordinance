using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magFieldRot : MonoBehaviour {

    public float rotSpeed = 10f;
    public float velocity = 1f;
    public float scalar = 1f;
    //public float 
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.forward * velocity * Time.deltaTime);
        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
	}
}
