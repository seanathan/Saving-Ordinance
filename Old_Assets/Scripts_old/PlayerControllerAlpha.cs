using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[AddComponentMenu ("Player Input Controls / Capacitance Tracker")]
public class PlayerControllerAlpha : MonoBehaviour {
    [Header("DEPRECATED")]

    public bool debugInvincible = false;

    public enum PlayerCondition {
        Waiting = 0,    //wait for new status
        Active = 1,
        Cinematic = 5,
        Crashed =10,
        OutOfBounds=11,
        Docked = 20,
     //   Landed = 21,
        Dead = 30
    }

    public PlayerCondition status;

    private static PlayerControllerAlpha player1;
    //private static bool playerIsActive = false;
    public GameObject PlayerOne;

    public bool OnRail = false;
    public GameObject activeRail;

    public bool ReEntry = false;

    public EnemyShipModular.mobAffiliation iff;

    [Header("Player Chassis")]
    
	public PlayerShip activeChassis;
    [Range(0,5)]
    public int playerChassis = 0;
    public PlayerShip[] chassis;



	[Header("Player Input Controls / Capacitance Tracker")]   

	[Header("Capacitance")]
	public static float charge = 500;
	public float chargeMax = 1000;
	public float recharge = 30;
	public float thrustCost = 10;
	public Slider capBar;

    const bool off = false;
    const bool on = true;

    [Header("Hull/Shields")]
    public float playerHP;
    
    public bool InlineShields;
    public bool shieldsUp = true;
    public static float shieldHP;
    public static float shieldMax;
    
    public float regen = 30;

    public float collDamage = 3f;
    
    [Header("Monitor Player Speed")]
    public float pSpeed = 1000.0f;

    [Header("Physics")]
    [Range(0f,5f)]
	public float cruiseDrag = 0.35f;
	public float initADrag = 1f;

	public float dockDrag;
	public float dockADrag;
    
    public Rigidbody playerRB;

    //Atmos Controls
    public float hoverToleranceX = 30.0f;
	public float hoverToleranceZ = 30.0f;

	public float hoverThrust = 100f;
	public float crashTimer = 2.0f;
	public float crashCountdown = 0.0f;
	private float dragLerp = 0.0f;
    
//	public bool docked = false;

    [Header("Movement Controls")]

    public float mV = 0f;
    public float mH = 0f;
    public float hV = 0f;
    public float hH = 0f;
    public float th = 0f;


    public float thrust = 0f;

    public bool rollStickOn = false;
    public bool rollForYaw = false;
    public bool tiltAndJoy = false;
    public bool freeRoll = false;
    public bool pitchOnArtificialUp = false;


    public float rollx = 0f;
    public float rolly = 0f;



    private float moveVertical = 0.0f;
    private float moveHorizontal = 0.0f;
    private float moveTurning = 0.0f;
    private float moveElev = 0.0f;
    
    private float xInput = 0.0f;
    private float yInput = 0.0f;
    private float xInputT = 0.0f;
    private float yInputT = 0.0f;

    [Header("Adjustments")]


    public float inputCurve = 1.5f;
    
    public float overBankCorrector = 1f;
    public float overBankMargin = 2.0f;
    


    public bool hover = false;
	public static bool hoverButton =  false;
	private float thrustButton = 0.0f;

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
    
    public float HPRestoreRate = 300f;

	[Header("Other Components")]
	private GameObject gyro;

//    private bool outofBounds;
	public GameObject goHomeWay;
    private MobileUI MobileControls;


    public bool takeover = false;
    public float takeoverScalar = 1f;
    public GameObject shipTaken;
    public EnemyShipModular dockedTo;
    private EnemyShipModular.mobCondition dockActiveState;
    public bool velocitySafety = false;

    public bool CinematicOverride = false;

    /////////////////////////////////////////////////////////////////////
    //  INITIALIZATION      /////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////
    
        /*
    void Awake() // Start
    {

      //  playerIsActive = false;

        InitPlayer();

        if (!gyro)
        {
            gyro = new GameObject("navGyro Null");
            gyro.transform.position = transform.position;
            gyro.transform.SetParent(transform);
        }


        shieldMax = activeChassis.shieldHP;
        playerRB = GetComponent<Rigidbody>();


        status = PlayerCondition.Active;

        
        ChassisSwitch(playerChassis);

        goHomeWay = GameObject.FindGameObjectWithTag("Boundary");

        if (MobileControls.MainStick != null)
            MobileControls.MainStick.gameObject.SetActive(true);

        activeChassis.ventralCruiseFlare.SetActive(false);

        hover = false;

        //Initialize Controls
        mH = 0;
        mV = 0;
        hH = 0;
        hV = 0f;
        th = 0f;
        
        moveHorizontal = 0;
        moveVertical = 0;
        moveTurning = 0;
        moveElev = 0;

        playerHP = getMaxHP();
     //   outofBounds = false;

       // cruiseDrag = playerRB.drag;
        initADrag = playerRB.angularDrag;

    }
    

    void OnValidate()
    {
        //   playerIsActive = false;
        
        InitPlayer();

        ChassisSwitch(playerChassis);
    }
    

    public static PlayerControllerAlpha GetActivePlayer()
    {
        
        if (player1)
            return player1;


        Debug.Log("Finding Player...");

        try
        {
            player1 = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerControllerAlpha>();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Player not found on frame " + Time.frameCount);
            Debug.Log(ex.Message);
            return null;

        }

        

        Debug.Log("Player found: " + player1.gameObject.name);
        return player1;

        
        
    }
    
    public void InitPlayer() {
        if (GetActivePlayer() == null)
            PlayerOne = gameObject;


        if (PlayerOne != null)
            player1 = PlayerOne.GetComponent<PlayerControllerAlpha>();

        if (MobileControls == null)
            MobileControls = GameObject.FindGameObjectWithTag("MobileUI").GetComponent<MobileUI>();
        
        GetActivePlayer();
    }
    

    void DockTraction(Collider otherColl = null)
    {
        //RaycastHit detector;
        //Physics.Raycast(transform.position, Vector3.down, out detector);
        
        if (otherColl != null)
        {
            if (otherColl.GetComponentInParent<EnemyShipModular>() != null && dockedTo == null)
            {
                dockedTo = otherColl.GetComponentInParent<EnemyShipModular>();
                if (dockedTo != null)
                    dockActiveState = dockedTo.condition;
            }

            dockedTo = otherColl.GetComponentInParent<EnemyShipModular>();
        }

        if (dockedTo != null)
        {
            if (status == PlayerCondition.Docked)
                dockedTo.condition = EnemyShipModular.mobCondition.anchored;
            else
                dockedTo.condition = dockActiveState;
        }

        if (otherColl == null)
        {
            dockedTo = null;

        }
        
    }

    public static float getMaxHP()
    {
        return GetActivePlayer().activeChassis.hullHP;
    }

    void Update()
    {
        //get upper limit of hp bar

        if (CinematicOverride)
            status = PlayerCondition.Cinematic;

        if (debugInvincible)
            playerHP = getMaxHP(); //prevent death if in debug

        if (!tiltControl) {
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

            capBar.value = (charge / chargeMax);
        }

        if (charge > chargeMax)
            charge = chargeMax;

        //Shield Bar
        if (shieldsUp)
        {
            if (shieldHP < shieldMax)
            {
                shieldHP += regen * Time.deltaTime;

            }

            if (InlineShields && ((playerHP + shieldHP) > getMaxHP()))
            {
                shieldHP = getMaxHP() - playerHP;

            }
            if (shieldHP > shieldMax)
            {
                shieldHP = shieldMax;
            }
        }
        else if (!shieldsUp)
            shieldHP = 0;


        //On Death
        if (playerHP < 1f)
        {
            if (GetComponentInChildren<ForwardFiring>() != null)
                GetComponentInChildren<ForwardFiring>().gameObject.SetActive(false);


            status = PlayerCondition.Dead;

            shieldHP = 0.0f;
            activeChassis.gameObject.SetActive(false);
            playerHP = 0f;
        }

        else if (status == PlayerCondition.Waiting)
            status = PlayerCondition.Active;

        //FLight Control Input from Player
        PlayerControls();

        //convert player input to ship controls
        FlightControls();

        if (crashCountdown > 0)
            Crashed();

        else
        {
        

             Docking();

            ///////////////////////////////////////////////////////////////
            //Normal Cruise //////////////////////////////////////////////

            //  DRAG    /////////////////////////////////////////////


            //      CAPACTITANCE OF THRUST  /////////////////////////////////////////
            charge -= (Mathf.Abs(thrust) * thrustCost * Time.deltaTime);


            //in-grav Controls
            GravFlight();

            //on-rail behavior


            //outofBounds return
            if (status == PlayerCondition.OutOfBounds)
            //if (outofBounds)
            {
                TurnTo(goHomeWay);
                playerRB.AddRelativeForce(0.0f, 0.0f, 10 * activeChassis.thrustPower);

                DialogueBox.Dialogue.text = "Returning to area of concern...";
            }

            //      STANDARD THRUST         /////////////////////////////////////////
            else if (OnRail)
            {
                activeRail = GameObject.FindGameObjectWithTag("Rail");
                RailFlight(activeRail);
            }
            else
                NormalFlight();



        }
        pSpeed = playerRB.velocity.magnitude;

    }

    public void DragControl(float controlledDrag)
    {
        cruiseDrag = controlledDrag;
    }

    void DynamicDrag()
    {
        playerRB.drag = cruiseDrag;
        playerRB.angularDrag = initADrag;

        if (status == PlayerCondition.Docked)
            playerRB.drag = dockDrag;
    }

  
//  BEGIN PLAYER CONTROL SEGMENT  ///////////

    float curveAdjust(float rawControl)
    {
        return (Mathf.Sign(rawControl) * Mathf.Pow(Mathf.Abs(rawControl), inputCurve));
    }

    void PlayerControls()
    {
        if (CinematicOverride)
            return;
        
        //Forward/Backward Movement
        th = (Input.GetAxis("Thrust") * 10) + thrustButton;// + Input.GetAxis("Fire1");
        //PROBLEMATIC//thrust = StickControl(thrust, th, 10); 
        thrust = th;

        //Engage Hover Controls / Maneuvering Thrusters
        hover = (Input.GetButton("Shift") || hoverButton);
        
        //  itialize all control inputs to 0 for this frame
        mH = 0;
        mV = 0;
        hH = 0;
        hV = 0f;
        rollx = 0f;
        rolly = 0f;      
       
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

    Vector3 TakeoverControl()
    {
        Vector3 controls = new Vector3(mV, mH);
        return controls * takeoverScalar;

    }



    void FlightControls()
    {
        moveTurning = StickControl(moveTurning, hH);
        moveElev = StickControl(moveElev, hV);
        moveVertical = StickControl(moveVertical, mV);

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
    

    void GravFlight()
    {
        //in-grav Controls
        activeChassis.ventralCruiseFlare.SetActive(playerRB.useGravity && Mathf.Abs(thrust) > 0.5);

        if (playerRB.useGravity)
            playerRB.AddForce(0.0f, hoverThrust, 0.0f);
        
    }

    void Crashed()
    {
        crashCountdown -= Time.deltaTime;

        moveHorizontal = StickControl(moveHorizontal);
        thrust = StickControl(thrust);
        moveVertical = StickControl(moveVertical);
        
        //disable thrusters too...
        moveTurning = StickControl(moveTurning);
        moveElev = StickControl(moveElev);

        GameLog.toLog("CRASHED!  Engines OFFLINE");

        //DRAG OVERRIDE
        playerRB.drag = dockDrag;
        dragLerp = crashCountdown / crashTimer;
        playerRB.angularDrag = Mathf.Lerp(dockADrag, 0f, dragLerp);
        playerRB.drag = Mathf.Lerp(dockDrag, 0f, dragLerp);

        if (crashCountdown >= 0.3)
            DialogueBox.PrintCrash("CRASHED! Engines Disabled." + "\n" + "Reinitializing in " + Mathf.Round(crashCountdown).ToString());
    }


    void TurnTo(GameObject wayPoint = null)
	{
		//lock gyro to nav lcoation
        if (wayPoint != null)
            gyro.transform.LookAt (wayPoint.transform);

		//lerp to match rotation

		//new heading
		float headingNav = gyro.transform.rotation.eulerAngles.x;
		float markNav = gyro.transform.rotation.eulerAngles.y;

		//original heading
		float heading0 = transform.rotation.eulerAngles.x;
		float mark0 = transform.rotation.eulerAngles.y;

		heading0 = Mathf.LerpAngle (heading0, headingNav, (activeChassis.bankSpeed / 50.0f )* Time.deltaTime);
		mark0 = Mathf.LerpAngle (mark0, markNav, (activeChassis.bankSpeed / 50.0f ) * Time.deltaTime);

		Quaternion newcourse = Quaternion.Euler(new Vector3 (heading0, mark0, 0.0f));

		transform.rotation = newcourse;
        
	}

    void TurnTo(Vector3 direction)
    {
        direction.Normalize();
        gyro.transform.forward = direction;
        TurnTo();
    }


    // Arcade-style rail flight

    void RailFlight(GameObject Rail = null)
    {
        if (Rail == null)
        {
            NormalFlight();
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
            TurnTo(Rail.transform.forward);
           
           // NormalFlight();
        }
        
        //clamp position to within a certain distance of a rail

        //rail has a certain amount of speed, and wiggle room


    }


    // Free-flight mode
    void NormalFlight(bool engageFlight = true)
    {
        if (!engageFlight)
            return;
        //      STANDARD THRUST         /////////////////////////////////////////

        DynamicDrag();

        playerRB.AddRelativeForce(moveTurning * activeChassis.dockThrust, moveElev * activeChassis.dockThrust, thrust * activeChassis.thrustPower, ForceMode.Force);

        pitchOnArtificialUp = freeRoll;

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
            transform.Rotate(Vector3.forward, activeChassis.bankSpeed * rollx * Time.deltaTime, Space.Self);
        
        //  PITCH and YAW//////////////////////////////////////////////////////////
        Space pitchSpace = Space.Self;
        Space yawSpace = Space.World;
        if (pitchOnArtificialUp)
        {
            pitchSpace = Space.Self;
            yawSpace = Space.Self;
        }

        transform.Rotate(Vector3.right, activeChassis.bankSpeed * moveVertical * Time.deltaTime, pitchSpace);
        transform.Rotate(Vector3.up, activeChassis.bankSpeed * moveHorizontal * Time.deltaTime, Space.Self);
        
        // OVERBANK RETURN
        OverBankReturn(!freeRoll);
    }


    void Docking()
    {
        if (status != PlayerCondition.Docked)
            return;

        Vector3 dockVelocity = Vector3.zero;
        
        if (thrust == 10)
        {
            thrust = thrust * 5;    //CATAPULT
                                    //display catapult advisement!
            MobileControls.GetComponentInChildren<inertialSafeties>().SafetyToggle(true, true);
        }

        else
            thrust = thrust / 1.5f;
        
        if (takeover)
        {
            //cut all maneuevering if takeover
            //lock camera to gyro if takeover

            moveHorizontal = 0;
            thrust = 0;
            moveVertical = 0;
            moveTurning = 0f;
            moveElev = 0f;

            //transform.rotation = dockTractionNull.transform.rotation;
        }
    }



    public void Restore()
    {
        
        if (playerHP < getMaxHP())
            playerHP = Mathf.MoveTowards(playerHP, getMaxHP(), HPRestoreRate * Time.deltaTime);
        if (charge < chargeMax)
            charge = Mathf.MoveTowards(charge, chargeMax, HPRestoreRate * Time.deltaTime);
    }
    
    void OnCollisionExit(Collision collisionInfo) 
	{

        //if (!outofBounds && !docked && crashCountdown <= 0) 
        if (crashCountdown <= 0 && status == PlayerCondition.Active)
		{
            //CRASH


			crashCountdown = crashTimer;
            Raimi.GetReactions().Panic("Impact!  Engines Restarting.");

            //  COLLISION DAMAGE    //////////////////////////////////////////////////
            //playerHP -= collDamage;
            playerHP -= Mathf.Round( collisionInfo.relativeVelocity.magnitude * collDamage);
//            NarrationWriter.PopDialogue(collisionInfo.relativeVelocity.magnitude.ToString());

        }
    }
    

    public void BulletHit(Ammunition bullet)
    {
        BulletHit(bullet.damage, bullet.getShooter());
    }


    public void BulletHit(float damage, GameObject shooter)
    {
        if (shieldsUp)
        {
            if (damage < shieldHP)
                shieldHP -= damage;
            else if (damage > shieldHP)
            {
            
                float shieldhit = damage - shieldHP;
                if (shieldHP < 1f)
                    playerHP -= shieldhit;

            }
        }
        else if (!shieldsUp)
            playerHP -= damage;
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Dock" || other.transform.tag == "Landing")
		{

            if (status == PlayerCondition.Active)
            //            if (!docked)
                MobileControls.GetComponentInChildren<throttleReset>().ThrottleReset();
            //docked = true;

            status = PlayerCondition.Docked;

            DockTraction(other);

            
            playerRB.useGravity = (other.transform.tag == "Landing");
        } 
		else 
		{
//			docked = false;
        }

		if (other.transform.tag == "Boundary" && status == PlayerCondition.OutOfBounds) {
            status = PlayerCondition.Waiting;

			//DialogueBox.Dialogue.text = "";
		}
	}

	void OnTriggerStay(Collider other)
	{
		
		if (other.transform.tag == "Landing" || other.transform.tag == "Dock")
		{
            //docked = true;

            status = PlayerCondition.Docked;

            DockTraction(other);


            playerRB.useGravity = (other.transform.tag == "Landing");

			//Quaternion normal = other.transform.rotation;
		}

        if (status == PlayerCondition.Docked)
        {
            if (other.GetComponentInParent<EnemyShipModular>() != null)
            {
                EnemyShipModular esm = other.GetComponentInParent<EnemyShipModular>();
                esm.takenover = takeover;
                if (takeover)
                {
                    esm.navGyro.transform.Rotate(TakeoverControl());
                    esm.TurnTo();
                }
            }
        }
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.transform.tag == "Landing" || other.transform.tag == "Dock") {
            //docked = false;

            //wait for new status

            status = PlayerCondition.Waiting;


            DockTraction();

            playerRB.useGravity = false;

            if (other.GetComponentInParent<EnemyShipModular>() != null)
            {
                EnemyShipModular esm = other.GetComponentInParent<EnemyShipModular>();
                esm.takenover = false;
                takeover = false;
            }
        }

		if (other.transform.tag == "Boundary") {

            status = PlayerCondition.OutOfBounds;

//            outofBounds = true;
			DialogueBox.Dialogue.text = "Returning to area of concern...";

		}
	}
    

    public void ChassisSwitch(int chass)
    {
        chass = (int)Mathf.Clamp(chass, 0, chassis.Length - 1);

        if (chassis.Length == 0) return;

        activeChassis = chassis[chass];

        foreach (PlayerShip ship in chassis)
            ship.gameObject.SetActive(ship == activeChassis);


        playerChassis = chass;



        //refresh weapons;
    }

    public static PlayerShip getActiveChassis()
    {
        PlayerControllerAlpha p = GetActivePlayer();
        if (p != null)
            return p.activeChassis;

        else
            Debug.Log("NO CHASSIS FOUND");
        
        return null;
    }

    public bool IsDocked()
    {
        return (status == PlayerCondition.Docked);
    }

    public static float getPlayerHP()
    {
        float hp = 0f;

        if (GetActivePlayer() != null)
            hp = GetActivePlayer().playerHP;

        if (hp < 1f)
            hp = 0;

        return hp;
    }

    public static void modPlayerHP(float change)
    {
        if (GetActivePlayer() != null)
            GetActivePlayer().playerHP += change;
        
        else
            Debug.Log("hit affects no player");
    }

    */
}
