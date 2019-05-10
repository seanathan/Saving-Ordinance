using UnityEngine;
using System.Collections;

public class gyroTest : MonoBehaviour {
    public Vector3 gyrostuff;
    public Vector3 gravitys;
    public bool gyroPresent;
    public string stuff;
    public bool onGyro;
	
	// Update is called once per frame
	void Update () {

        if (onGyro)
        {
            Input.gyro.enabled = true;
        }
        gyrostuff = Input.gyro.attitude.eulerAngles;
        gravitys = Input.gyro.gravity;
        transform.rotation = Input.gyro.attitude;
        gyroPresent = SystemInfo.supportsGyroscope;
        stuff = Input.gyro.enabled.ToString();
        
    }
}
