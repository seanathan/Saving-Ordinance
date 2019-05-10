using UnityEngine;
using System.Collections;

public class Angle180 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public static float DegreeClamp(float degreeRaw)    //corrects one single euler angle
    {
        /*
        if (Mathf.Abs(degreeRaw) > 180)
            degreeRaw -= 360 * Mathf.Sign(degreeRaw);
            */

        if (degreeRaw > 180)
            degreeRaw -= 360;

        else if (degreeRaw < -180)
            degreeRaw += 360;

        return degreeRaw;
    }


    public static Vector3 EulerFixer(Vector3 eulerToCorrect) // Corrects an euler to be within -180 to 180 on all axes
    {

        eulerToCorrect.x = DegreeClamp(eulerToCorrect.x);
        eulerToCorrect.y = DegreeClamp(eulerToCorrect.y);
        eulerToCorrect.z = DegreeClamp(eulerToCorrect.z);

        return eulerToCorrect;
    }
}
