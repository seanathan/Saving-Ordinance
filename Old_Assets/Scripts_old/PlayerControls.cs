using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    //include camera controls 

    public string GetChassisByAlias;
    public LameShip activeVessel;
    private static PlayerControls pc;
    
    public bool debugInvincible = false;

    public enum PlayerCondition
    {
        Waiting = 0,    //wait for new status
        Active = 1,
        Cinematic = 5,
        OutOfBounds = 11,
        Docked = 20,
        //   Landed = 21,
        Dead = 30
    } 

    public PlayerCondition status;
    
    public bool OnRail = false;
    public GameObject activeRail;
    
    //CAPACITANCE
    public float charge = 500;
    public float chargeMax = 1000;
    public float recharge = 30;
    public float thrustCost = 10;
    const bool off = false;
    const bool on = true;


    //Atmos Controls
    public float crashTimer = 2.0f;
    public float crashCountdown = 0.0f;

    [Header("Movement Controls")]

    //inputs
    public float mV = 0f;
    public float mH = 0f;
    public float hV = 0f;
    public float hH = 0f;
    public float th = 0f;
    public bool hoverButton = false;
    public float thrustButton = 0f;
    
    public bool rollStickOn = false;
    public bool rollForYaw = false;
    public bool tiltAndJoy = false;
    public bool freeRoll = false;
    
    public bool hover = false;

	//movement vars
	private float moveVertical = 0.0f;
	private float moveHorizontal = 0.0f;
	private float moveTurning = 0.0f;
	private float moveElev = 0.0f;
	private float thrust = 0f;

	public float rollx = 0f;
    public float rolly = 0f;

    private float xInput = 0.0f;
    private float yInput = 0.0f;
    private float xInputT = 0.0f;
    private float yInputT = 0.0f;
    
    [Header("Adjustments")]
    
    public float inputCurve = 1.5f;

    public float overBankCorrector = 1f;
    public float overBankMargin = 2.0f;

    //Acceleration Input
    private float rollControlTimeOut = 10f;
    private float rCTOcountdown = 0f;

    public bool tiltControl = false;
    public float xClamp = 1.0f;
    public float yClamp = 1.0f;
    public float xAcc = 3f;
    public float yAcc = -3f;
    public float yOffset = 0.6f;
    public float tiltDeadZoneX = 0.01f;
    public float tiltDeadZoneY = 0.01f;
    public float tiltRounder = 1000;
    public float accRate = 6f;

    public float stickRestRate = 3f;    // 2.5? 3?


    public GameObject goHomeWay;
    public GameObject rail;

    private MobileUI MobileControls;

    public EnemyShipModular dockedTo;
    
    public bool CinematicOverride = false;

    public void InitPlayer()
    {
        if (MobileControls == null)
            MobileControls = GameObject.FindGameObjectWithTag("MobileUI").GetComponent<MobileUI>();
        
    }


    public static bool cockpitMode(bool onCockpit)
    {
        if (getPlayerShip() != null)
        {
            if (getPlayerShip().getShipGimbal() != null)
                return (getPlayerShip().getShipGimbal().cockpitMode(onCockpit));
        }

        //else
        return false;

        
    }

    public static float getPlayerHP()
    {
        if (getPlayerShip() != null)
            return getPlayerShip().getHealth().currentHP;

        else
            return 1;
    }

    public static PlayerControls GetActivePlayer()
    {
        if (pc == null)
        {
            pc = Transform.FindObjectOfType<PlayerControls>();
        }

        return pc;
    }
    private void Awake()
    {
        pc = this;
    }

    private void OnValidate()
    {
        pc = this;
    }

    public static bool isPlayerDead()
    {
        if (GetActivePlayer() != null)
            return (GetActivePlayer().status == PlayerCondition.Dead);

        return false;
    }

    public static float getPlayerVelocity()
    {
        if (getPlayerShip() != null)
            return getPlayerShip().getVelocity();

        else return 0f;
    }

    void Update()
    {
        InitPlayer();

        if (activeVessel == null)
            return;

        if (CinematicOverride)
            status = PlayerCondition.Cinematic;
        else if (status == PlayerCondition.Waiting)
            status = PlayerCondition.Active;
        

        if (debugInvincible)
            activeVessel.restoreShip(); //prevent death if in debug

        if (!tiltControl)
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.AutoRotation;

        }
        else if (tiltControl)
        {
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

        }
        
        //Capacitance Bar
        if (charge < chargeMax)
        {
            charge += recharge * Time.deltaTime;

            //capBar.value = (charge / chargeMax);
        }

        if (charge > chargeMax)
            charge = chargeMax;
        
        //On Death
        if (activeVessel.condition == LameShip.shipCondition.dead)
        {
            if (GetComponentInChildren<ForwardFiring>() != null)
                GetComponentInChildren<ForwardFiring>().gameObject.SetActive(false);
            
            status = PlayerCondition.Dead;

        }

        if (CinematicOverride)
            return;

        //  itialize all control inputs to 0 for this frame
        th = 0;
        mH = 0;
        mV = 0;
        hH = 0;
        hV = 0f;
        rollx = 0f;
        rolly = 0f;

        //Forward/Backward Movement
        th = Input.GetAxis("Thrust") + thrustButton;// + Input.GetAxis("Fire1");
        //PROBLEMATIC//thrust = StickControl(thrust, th, 10); 
        //thrust = th;

        //Engage Hover Controls / Maneuvering Thrusters
        hover = (Input.GetButton("Shift") || hoverButton);


        //Normal Flight


        if (!tiltAndJoy)
            mH = GetTiltControl().x;

        mV = GetTiltControl().y;



        //add trackpad input if enabled
        if (MobileControls.MainStick)
        {
            if (!tiltControl)
            {
                mH += MobileControls.MainStick.position.x;
                mV += MobileControls.MainStick.position.y;
            }
            else if (tiltControl)
            {
                hH += MobileControls.MainStick.position.x;
                hV += MobileControls.MainStick.position.y;
            }
        }

        //square(or more) the input to give a smooth curve from -1 to 1)
        //mH = Input.GetAxis("Horizontal") + Mathf.Sign(mH) * Mathf.Pow(Mathf.Abs(mH), inputCurve);

        //mV = (PauseMenu.pitchInvert * Input.GetAxis("Vertical")) + Mathf.Sign(mV) * Mathf.Pow(Mathf.Abs(mV), inputCurve);
        mH = Input.GetAxis("Horizontal") + curveAdjust(mH);

        mV = Input.GetAxis("Vertical") + curveAdjust(mV);


        //PITCH INVERSION
        mV *= PauseMenu.pitchInvert;


        //Maneuvering Flight Controls

        if (hover)  // for single-stick flight, momentary switch for hoverthrusters
        {
            hH = mH;
            hV = mV;

            //if hover button is engaged, swap main joystick to the hover controls, and clear main joystick
            mH = 0;
            mV = 0;
        }

        FlightControls();

        if (crashCountdown > 0)
        {
            Crashed();
            //return; //no further code executed
            DeadStick();
        }


        charge -= (Mathf.Abs(thrust) * thrustCost * Time.deltaTime);

        if (status == PlayerCondition.Docked)
        {
            // Docking();
        }


        ///////////////////////////////////////////////////////////////
        //Normal Cruise //////////////////////////////////////////////

        //  DRAG    /////////////////////////////////////////////


        //      CAPACTITANCE OF THRUST  /////////////////////////////////////////
        
        //in-grav Controls
        activeVessel.GravFlight();


        if (status == PlayerCondition.Active)
        {
            NormalFlight();
            
            if (OnRail)
            {
                activeRail = GameObject.FindGameObjectWithTag("Rail");
                RailFlight(activeRail);
            }
        }

        //outofBounds return
        else if (status == PlayerCondition.OutOfBounds)
        {
            activeVessel.NavTo(goHomeWay);
            // activeVessel.TurnTo(goHomeWay);
            // activeVessel.ForwardThrust();

            DialogueBox.Dialogue.text = "Returning to area of concern...";
        }


    }

    void ThrottleControl()
    {

    }

    public static LameShip getPlayerShip()
    {
        if (GetActivePlayer() != null)
        {
            return GetActivePlayer().activeVessel;
        }

        return null;
    }
    
    // Free-flight mode
    void NormalFlight(bool engageFlight = true)
    {
        if (!engageFlight)
            return;

        //      STANDARD THRUST         /////////////////////////////////////////

        activeVessel.DynamicDrag();

        activeVessel.EvasiveThrust(moveTurning, moveElev);

        activeVessel.throttle = thrust;

        //silly simple check...
        rollStickOn = (GetJoystick2().magnitude != 0);

        //      ROLL CONTROLS           /////////////////////////////////////////
        if (rollStickOn || rollForYaw)
        {
            //Alt Joystick Roll
            //get 2nd joystick for ROLL if desired

            Vector2 altJoy = GetJoystick2();
            rollx += -altJoy.x; //add joystick x value to rollx
            rolly += altJoy.y;  //add joystick y value to rolly

            if (Mathf.Abs(rolly + rollx) > 0f)
            {
                //reset freeroll countdown
                rCTOcountdown = rollControlTimeOut;

            }
        }

        //rotate player on z axis if engaged
        if (rCTOcountdown > 0f)
        {
            rCTOcountdown -= Time.deltaTime;
            freeRoll = true;
        }

        if (freeRoll)
            activeVessel.Roll(rollx);

        //  PITCH and YAW//////////////////////////////////////////////////////////

        activeVessel.Pitch(moveVertical);
        activeVessel.Yaw(moveHorizontal);
        
        // OVERBANK RETURN
        OverBankReturn(!freeRoll);
    }
    

    void Crashed()
    {
        crashCountdown -= Time.deltaTime;
        
        GameLog.toLog("CRASHED!  Engines OFFLINE");

        //DRAG OVERRIDE

        float dragLerp = 10f * ( 1 - (crashCountdown / crashTimer));

        activeVessel.DynamicDrag(dragLerp);

        if (crashCountdown >= 0.3)
            DialogueBox.PrintCrash("CRASHED! Engines Disabled." + "\n" + "Reinitializing in " + Mathf.Round(crashCountdown).ToString());

    }

    void RailFlight(GameObject Rail = null)
    {
        if (Rail == null)
        {
            //NormalFlight();
            return;
        }

        Ray railForward = new Ray(Rail.transform.position, Rail.transform.forward);

        //aim on axis, give window of about 30 degrees in any direction
        //check if within tolerance

        float tol = 30f;

        float offAxis = Vector3.Angle(transform.forward, Rail.transform.forward);

        //tolerance grows if close to axis, up to 75 degrees

        if (offAxis > tol)
        {
            activeVessel.TurnTo(Rail.transform.forward);
            
            // NormalFlight();
        }

        //clamp position to within a certain distance of a rail

        //rail has a certain amount of speed, and wiggle room


    }

    float curveAdjust(float rawControl)
    {
        return (Mathf.Sign(rawControl) * Mathf.Pow(Mathf.Abs(rawControl), inputCurve));
    }
    
    Vector2 GetJoystick2()
    {
        float controlx = 0f;
        float controly = 0f;

        //thrusters on with secondary control
        //thrusters on
        controlx = Input.GetAxis("Strafe");
        controly = Input.GetAxis("Elevate");


        //square(or more) the input to give a smooth curve from -1 to 1)
        controlx = curveAdjust(controlx);

        controly = curveAdjust(controly);

        Vector2 controlOut = new Vector2(controlx, controly);

        if (controlOut.magnitude != 0)
            rCTOcountdown = rollControlTimeOut;

        return controlOut;
    }

    float StickControl(float control, float getStick = 0f, float bound = 1f)
    {
        //control: value of current control
        //getStick: value of Inputs toward control
        //bound: max and min value for control;

        //slow to reset
        if (getStick == 0)
            control = Mathf.MoveTowards(control, 0f, stickRestRate * Time.deltaTime);
        else
        {

            getStick = Mathf.Clamp(getStick, -bound, bound);
            control = Mathf.MoveTowards(control, getStick, stickRestRate * Time.deltaTime);
        }

        return control;
    }

    public static bool isTiltControlled()
    {
        return GetActivePlayer().tiltControl;
    }

    Vector2 GetTiltControl()
    {
        if (tiltControl)
        {
            //ypitch
            if ((Mathf.Abs(Input.acceleration.y) - yOffset) > tiltDeadZoneY || (Mathf.Abs(Input.acceleration.y) - yOffset) < -tiltDeadZoneY)
            {
                yInputT = yAcc * (Mathf.Abs(Input.acceleration.y) - yOffset);
                //y offset by .5 / 45 degrees
            }

            else
            {
                yInputT = 0.0f;
            }

            //xroll
            if (Mathf.Abs(Input.acceleration.x) > tiltDeadZoneX)
            {
                xInputT = xAcc * Input.acceleration.x;

            }
            else
            {
                xInputT = 0.0f;
            }

            //move gradually
            yInput = Mathf.Clamp(Mathf.Lerp(yInput, yInputT, accRate * Time.deltaTime), -yClamp, yClamp);
            xInput = Mathf.Clamp(Mathf.Lerp(xInput, xInputT, accRate * Time.deltaTime), -xClamp, xClamp);

        }
        else
        {
            yInput = 0.0f;
            xInput = 0.0f;
            
            
            if (MobileControls.MainStick != null)
                MobileControls.MainStick.gameObject.SetActive(true);

        }

        return new Vector2(xInput, yInput);
    }

    //UI Controls


    public void Thruster(float button)
    {
        thrustButton = button;
    }

    //momentary hover
    public void HoverMomentary(bool button)
    {
        hoverButton = button;
    }



    //receive clicks for context
    public void TiltToggle()
    {

        if (tiltControl == false)
        {
            tiltControl = true;
            //reset yOffset
            yOffset = Mathf.Abs(Input.acceleration.y);

            yInput = 0.0f;
            xInput = 0.0f;
        }

        else if (tiltControl == true)
        {
            tiltControl = false;
            yInput = 0.0f;
            xInput = 0.0f;

        }
    }


    //  END PLAYER CONTROL SEGMENT  ///////////

    public void SwapYawForRoll()
    {
        rollForYaw = !rollForYaw;
    }
 
    void FlightControls()
    {
        moveHorizontal = StickControl(moveTurning, hH);
        moveElev = StickControl(moveElev, hV);
        moveVertical = StickControl(moveVertical, mV);

        thrust = StickControl(thrust, th);


        if (!rollForYaw)
        {
            moveHorizontal = StickControl(moveHorizontal, mH);
            rollx = StickControl(rollx);
        }
        else if (rollForYaw)
        {
            moveHorizontal = StickControl(moveHorizontal);
            rollx = StickControl(rollx, -mH);
        }


    }

    public void DeadStick()
    {
        moveTurning = 0f;
        moveElev = 0f;

        moveHorizontal = 0f;
        moveVertical = 0f;

        thrust = 0f;
        if (getPlayerShip() != null)
            getPlayerShip().throttle = 0f;

        hover = false;
    }


    void OverBankReturn(bool bankReset = true)
    {
        if (!bankReset)
            return;

        float overBanked = transform.localRotation.eulerAngles.z;

        if ((Mathf.Abs(overBanked) > overBankMargin && Mathf.Abs(mH) < 0.2 && Mathf.Abs(mV) < 0.2))
        {
            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0.0f, overBankCorrector * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newAngle);
        }
    }

    

}
