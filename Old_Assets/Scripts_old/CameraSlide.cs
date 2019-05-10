using UnityEngine;
using System.Collections;

public class CameraSlide : MonoBehaviour {


    public float slideForce = 4.0f;
    public float HovSlideForce = 4.0f;
    public float lorentzForce = 0.5f;
    public Vector3 offset;
    public float followSpeed = 1.0f;
    public float slideSpeed = 1.0f;
    private float slideX = 0.0f;
    private float slideY = 0.0f;
    private float slideZ = 0.0f;
    public PlayerControls controls;
    
	
	// Update is called once per frame
	void FixedUpdate () {

        controls = PlayerControls.GetActivePlayer();



        //slide with target


        if (controls == null)
            return;

        if (controls.hover == false)
        {

            slideX = Mathf.Lerp(slideX, controls.mH * slideForce, slideSpeed * Time.deltaTime);
            slideZ = Mathf.Lerp(slideZ, controls.th * lorentzForce, followSpeed * Time.deltaTime);


            slideY = Mathf.MoveTowards(slideY, 0.0f, slideSpeed * Time.deltaTime);

            

        }


        else
        {
            slideX = Mathf.Lerp(slideX, controls.hH * HovSlideForce, slideSpeed * Time.deltaTime);
            slideY = Mathf.Lerp(slideY, controls.hV * HovSlideForce, slideSpeed * Time.deltaTime);
            slideZ = Mathf.Lerp(slideZ, controls.th * lorentzForce, followSpeed * Time.deltaTime);

        }

        Vector3 followPos = new Vector3(slideX, slideY, slideZ);
        transform.localPosition = offset + followPos;


    }
}
