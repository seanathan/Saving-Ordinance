using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class GunCamera : MonoBehaviour
{
    public static GunCamera activeCamera;
    public Vector3 scalar;
    public Vector3 camSize;

    public GameObject CameraFocus;
    public float wheelPos = 0.0f;
    public bool lockToGimbal = false;


    public float xTurn = 75f;
    public float yTurn = 90f;
    public float zTurn = 75f;

    private float mark1 = 0.0f;
    private float heading1 = 0.0f;
    private float bank1 = 0.0f;

    // public float minVel = 20.0f;
    public float lookdead = 0.2f;

    public float contF = 2.0f;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    public float mouseF = 0.2f;


    public GameObject target;
    public bool freeLook;
    public float lookTimeMax = 2.0f;
    public float lookTimer;
    public float lerpTimer = 0.0f;
    public float lerpTime = 1.0f;
//    private Quaternion lastlook;

    public bool rayInterlock = false;
    public bool lookTarget = false;

    public float cameraRoll = 0.5f;

    public float stickF = 50.0f;
    public float stickSensitivity = 1.5f;

    public float stickDeadStart = 0.3f;

    public static bool cockpitView = false;

    public MobileUI MobileControls;
    public bool camDisabler = false;
    public bool camReEnable = false;
    private bool fixedFrame = false;
    public bool rotateOnlyOnFixedFrames = true;
    public bool FirestickActuallyWorks = false;

    public void CamDisable()
    {
        //dead man switch program
        camDisabler = true;

    }
        
    void Start()
    {
        
        freeLook = false;
        target = ScoreKeeper.getPlayerAlive();
        lookTarget = false;

        //clear parent
        //        transform.SetParent();
    }
    
    public static bool isLockedOn()
    {
        if (activeCamera == null)
            return false;

        else if (activeCamera.lookTarget)
            return true;

        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        activeCamera = this;

        if (storyController.Narrating && storyController.disableCam)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
            camReEnable = true;
        }
        //dead man switch, will reenable after narration done
        else if (camReEnable)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            camReEnable = false;
        }



        //        if (MobileControls.FireStick != null) 
        //		{
        //			if (ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha> ().tiltControl == true) {
        //                MobileControls.FireStick.gameObject.SetActive (true);
        //			} 
        //
        //			else {
        //                MobileControls.FireStick.gameObject.SetActive (false);
        //
        //			}
        //		}

        float mouseW = Input.mouseScrollDelta.y;
        wheelPos = wheelPos - mouseW;

        //max wheel
        if (wheelPos > 9)
        {
            wheelPos = 9;
        }
        //min wheel
        if (wheelPos < -9)
        {
            wheelPos = -9;
        }

        //zoom alter

        /*
        if (ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().docked)
        {

            camSize = scalar * (1 + (0.1f * 50.0f));
        }
        else    */
        {
            camSize = scalar * (1 + (0.1f * wheelPos));
        }

        transform.localScale = camSize;

        cockpitView = (wheelPos < -3);
        //ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().activeChassis.cockpit.SetActive(cockpitView);
        //ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().activeChassis.shipModel.SetActive(!cockpitView);

        PlayerControls.cockpitMode(cockpitView);

        if (lockToGimbal && PlayerControls.getPlayerShip() != null)
        {
            if (PlayerControls.getPlayerShip().getShipGimbal() != null)
                CameraFocus = PlayerControls.getPlayerShip().getShipGimbal().gameObject;
            else
            {
                lockToGimbal = false;
                Debug.Log("No gimbal found");
            }
        }
        else if (!lockToGimbal)
        {
			CameraFocus = PlayerControls.getPlayerShip().gameObject;
		}

        else

            target = DialogueBox.tracking;



    }

    void FixedUpdate()
    {
        fixedFrame = true;
    }


    //	Simple Follow

    void LateUpdate()
    {
        //follow position
        //if (!transform.IsChildOf(CameraFocus.transform)) 
          
		transform.position = CameraFocus.transform.position;



        if (Input.GetButtonDown("LookAt"))
        { lookTarget = true; }


        if (Input.GetButtonUp("LookAt"))
        { lookTarget = false; }


        if (MobileControls.MainStick && FirestickActuallyWorks)
        {
            if (MobileControls.MainStick.fireEngaged)
            {


                //change mainstick.position for firestick.position when working
                if (lookTarget == false && Mathf.Abs(MobileControls.MainStick.position.magnitude) > stickDeadStart)
                {
                    //            if (Input.GetButton("Fire3") || Mathf.Abs(Input.GetAxis("VerticalLook")) > lookdead || Mathf.Abs(Input.GetAxis("HorizontalLook")) > lookdead || (Input.touchCount > 0 && Input.GetTouch(fireStick.touchNum).phase == TouchPhase.Moved))
                    freeLook = true;

                    lookTimer = lookTimeMax;

                    float lookHorizontal = (contF * Input.GetAxis("HorizontalLook")) + (MobileControls.MainStick.position.x * stickF);
                    float lookVertical = (contF * Input.GetAxis("VerticalLook")) + (MobileControls.MainStick.position.y * stickF);

                    mouseX = Mathf.Sign(lookHorizontal) * Mathf.Pow(Mathf.Abs(lookHorizontal), stickSensitivity);

                    mouseY = PauseMenu.pitchInvert * Mathf.Sign(lookVertical) * Mathf.Pow(Mathf.Abs(lookVertical), stickSensitivity);

                    transform.Rotate(mouseY, mouseX, 0.0f);
                }
                else
                    TurnTo(CameraFocus);
                
                //MobileControls.FireControlScript.Trigger();

            }
            //else if (MobileControls.FireStick.engaged == false)
           
            //Pull Trigger
           //     MobileControls.FireControlScript.Trigger(MobileControls.FireStick.engaged);
        }
        if (freeLook == false)
        {
            mouseX = 0.0f;
            mouseY = 0.0f;

            if (!lookTarget || target == null)
            {
                //    target = CameraFocus;


                //try only running with player
                if (fixedFrame)
                {
                    TurnTo(CameraFocus);
                    if(rotateOnlyOnFixedFrames)
                        fixedFrame = false;
                }


            }

            if (lookTarget && target != null)
            {
                LockOn();

            }
        }



        if (lookTimer < 0.0f)
        {
            freeLook = false;

        }


        //freelooking on mouse

        if (freeLook == true)
        {

            lookTimer = lookTimer - Time.deltaTime;

            if (lookTimer < 1.0f)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, CameraFocus.transform.rotation, (1 - lookTimer));

            }
        }


        transform.GetComponentInChildren<HUDTracker>().RunWithCamera();

        EnvironmentCamera.env.transform.rotation = transform.rotation;
    }

    //navigation


    //lerp follo
    void TurnTo(GameObject wayPoint)
    {

        /*
        if (player.GetComponentInParent<PlayerControllerAlpha>().docked)
        {
            Quaternion newcourse = Quaternion.Euler(new Vector3(45.0f, wayPoint.transform.rotation.eulerAngles.y, -player.transform.localRotation.z));

            transform.localRotation = newcourse;
        }

        else    */
        {

            //new heading
            float markNav = wayPoint.transform.rotation.eulerAngles.x;
            float headingNav = wayPoint.transform.rotation.eulerAngles.y;
            float bankNav = wayPoint.transform.localRotation.eulerAngles.z;

            if (mark1 == 270.0f)
            {
                mark1 = 280.0f; 
            }

            //active heading
            heading1 = Mathf.LerpAngle(heading1, headingNav, yTurn * Time.deltaTime);
            mark1 = Mathf.LerpAngle(mark1, markNav, xTurn * Time.deltaTime);
            bank1 = Mathf.LerpAngle(bank1, bankNav, zTurn * Time.deltaTime);

            Quaternion newcourse = Quaternion.Euler(new Vector3(mark1, heading1, bank1 * cameraRoll));

            //			transform.localRotation = player.transform.rotation;
            //transform.localRotation = newcourse;
            transform.rotation = newcourse;
        }
    }

    public void cameraDrag(bool interlock)
    {
        rayInterlock = interlock;
    }

    public void trackToggle()
    {
        if (lookTarget == true)
        {
            lookTarget = false;
        }
        else if (lookTarget == false)
        {
            lookTarget = true;
        }
    }

    public void trackEnemy()
    {

        lookTarget = true;

    }

    public void trackPlayer()
    {

        lookTarget = false;

    }

    void LockOn()
    {
        transform.LookAt(target.transform);

       // transform.rotation = GetComponentInChildren<HUDTracker>().targetingPick.transform.rotation;
    }
}
