using UnityEngine;
using System.Collections;

public class asteroidRotator : MonoBehaviour {
    public float rotSpeed = 1f;
    public GameObject rotateGuide;
    public Vector3 rotateSweep;

	// Update is called once per frame
	void Update () {
        if (rotateGuide != null)
            rotateSweep = rotateGuide.transform.localRotation.eulerAngles;
         transform.Rotate(rotateSweep * rotSpeed * Time.deltaTime);
    }
}
