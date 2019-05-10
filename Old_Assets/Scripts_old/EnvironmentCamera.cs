using UnityEngine;
using System.Collections;

public class EnvironmentCamera : MonoBehaviour {

    private Camera[] cams;
    public Camera activeCam;
    public float followSpeed = 10.0f;
    public bool followPlayer = false;
    public static EnvironmentCamera env;
    //follow the main camera

    //main cam will have depth between 0 and 10
    //env cam will be less than 0;

    public Camera GetActiveCam()
    {
        cams = Camera.allCameras;
        Camera foundCam = Camera.main;
        if (foundCam == null)
            return null;

        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i].depth > foundCam.depth && cams[i].depth <= 10)
                foundCam = cams[i];
        }

    //    if (foundCam.depth < 0)
      //      return null;    //no main camera

        return foundCam;
    }

    public void CameraMatch()
    {
        if (activeCam != null)
            transform.rotation = activeCam.transform.rotation;

    }

    void Start()
    {
        env = this;

        activeCam = GetActiveCam();
    }

    void OnValidate()
    {
        env = this;
        CameraMatch();
    }

    void LateUpdate()
    {
        //GetComponent<Camera>().Render();
        activeCam = GetActiveCam();
      //  CameraMatch();
    }    
}
