using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LameShip : MonoBehaviour {

    /*  Required Components
     *  RigidBody
     *  Colliders
     *  SpaceShip script
     *  Ship Gimbal
     *  
     *  ADDITIONAL Components
     *  ship gimbal
     *  ship breaks
     *  Behavioral Triggers  
     *  Engines
     *  Thrusters
     *  Animations
     *  Death Effects
     *  Color / Feature Changes on IFF
     */
     

    public string alias = "shipName";
	
    public struct health
    {
        public int maxHP;
        public int currentHP;
        public float ratio;
    }

    private health shipHealth;
    

    public enum shipCondition
    {
        //drag-free
        waiting = 0,
        dead = 1,
        disabled = 2,

        adrift = 4,

        //Rigidbody Frozen   
        anchored = 5,
        
        //crashed = 8,
        //Rigidbody  normal
       // idle = 9,
        active = 10, //running: 10+

        takeover = 20
    }
    public shipCondition condition;

    public enum mobAffiliation
    {
        //GENRALLY HARMLESS 0-9

        NonThreat = 0,      //generic nonthreat
        Civillian = 1,
        //Disabled =1,
        //Salvage = 2,
        //Objective = 3,

        // ALLIED MILITARY 10-19
        Ally = 10,          //generic ally
        Raysian = 11,

        // RRELL OR OTHER THREATS 20-29
        Threat = 20,        //generic threat
        Rrell = 21
        //Boss = 21,
    }

    public mobAffiliation iff;

    public enum shipController
    {
        mob,
        player,
        other,
        waiting
    }
    public shipController pilot;

    public enum navMode
    {
        idle,
        patrol,
        waypoint,
        combat
    }

    public navMode guidance;
    
    public enum mobPriority
    {
        normal,
        objective,
        boss
    }
	
    public mobPriority priority;

    public int soulsOnboard = 1;
    public int soulsAlive = 1;

    [Header("Specifications")]
    public float mainEnginePower = 1;
    public float evasivePower = 1;
    public float turningPower = 1;

    public float hoverThrust = 100f;
    
    public int hullHP = 100;
    private float eHP = 1;    //current HP

    public float shieldHP = 0;
    public int shieldHPMax = 0;

    public float shieldRegenPerSecond;
    public bool shieldsUp = false;

    public float maxSafeVelocity = 200;

    
    [Range(-.5f,1f)]
    public float throttle = 0f;
    //private float realThrottle;

    public float CollisionDamage = 10;

    [Header("Ship Components")]

    public EnemyEngineModule[] engines;
    public EnemyGunTurret[] weapons;
    public GameObject weaponsKit;
    
    public Transform[] hardPoints;
    
    private GameObject navGyro; //turn this to move the ship

    public GameObject lowHPEffect;
    public GameObject deathEffect;
    public GameObject disabledEffect;
    
    private Vector3 lastVelocity;
    private Vector3 deltaVelocity;
    private Rigidbody rb;
    public float normalDrag = 0.25f;
    public float normalADrag = 0.35f;

    public string mobDialogue = "";

    public bool wasHit;     //flag that shows ship was recently hit

    public Animator battleAnim;         //host of animator


    public SetOfGameObjects ReEntryBurn;
    public GameObject ventralCruiseFlare;
    public ShipGimbal shipGimbal;
    public GameObject cockpit;

    private GameObject NavigationWayPoint;

    public GameObject DockingPort;


    //movement vars
    [Header("Manual Controls")]
    public float moveVertical = 0.0f;
    public float moveHorizontal = 0.0f;
    public float moveLateral = 0.0f;
    public float moveElev = 0.0f;
    public float thrust = 0f;
    public float rollx = 0f;
    public float rolly = 0f;

    private float rollControlTimeOut = 10f;
    private float rCTOcountdown = 0f;

    public float overBankCorrector = 1f;
    public float overBankMargin = 2.0f;

    public bool freeRoll = false;
	

    void Start()
    {
        ScoreKeeper.soulsMax += soulsOnboard;
        soulsAlive = soulsOnboard;
        
        ConditionUpdate();
		SpaceShip newship;
    }

    public Vector3 getAccelForce()
    {
        if (condition != shipCondition.active || getRigidBody() == null)
            return Vector3.zero;
        
        if (UserInfo.checkRealisticThrusterEffects())
        {
            //calculate rocket thrust
            if (lastVelocity != getRigidBody().velocity)
            {
                //get new pull only if hasn't been done this frame
                deltaVelocity = getRigidBody().velocity - lastVelocity;
                lastVelocity = getRigidBody().velocity;

            }

        }
        else
            return (transform.forward * throttle);

        return deltaVelocity;
        
    }

    public ShipGimbal getShipGimbal()
    {
        if (shipGimbal == null)
            shipGimbal = GetComponentInChildren<ShipGimbal>();
        
        return shipGimbal;
    }
    

    public health getHealth()
    {
        return new health();

        shipHealth.currentHP = (int)eHP;
        shipHealth.maxHP = hullHP;
        shipHealth.ratio = eHP / hullHP;

        return shipHealth;
    }

    public void initMob()
    {
        getNavGyro();
        getShipGimbal();
    }

    private void Update()
    {
        ConditionUpdate();
        initMob();


        //Shield Recharge
        if (shieldsUp)
        {
            if (shieldHP < shieldHPMax)
            {
                shieldHP += shieldRegenPerSecond * Time.deltaTime;
            }

            if (shieldHP > shieldHPMax)
            {
                shieldHP = shieldHPMax;
            }
        }
        else if (!shieldsUp)
            shieldHP = 0;

        if (pilot == shipController.mob)
        {
            if (DockingPort != null )
            {
                if (condition > shipCondition.anchored)
                    DockHere(DockingPort);
            }
        }
    }

    public GameObject getNavGyro()
    {
        if (navGyro == null)
        {
            navGyro = new GameObject("navGyro Null");
            navGyro.transform.position = transform.position;
            navGyro.transform.SetParent(transform);

        }

        return navGyro;

    }

    public float EnginePowerAvailable() //returns percentage 0 to 1
    {
        if (engines == null)
            engines = GetComponentsInChildren<EnemyEngineModule>();

        if (engines.Length == 0)
            return 1;

        float powerAvailable = 0;

        //check health and power of each engine
        for (int i = 0; i < engines.Length; i++)
        {
            powerAvailable += engines[i].enginePower / engines.Length;
        }

        if (powerAvailable == 0)
            EnginesDisabled();

        return powerAvailable;
    }

    void EnginesDisabled()
    {
        if (condition >= 0)
        {

            ShipDialogue(gameObject.name + " Engines: DISABLED");

            //Raimi Commentary
            NarrationWriter.PopDialogue(string.Format("<b><i>{0}</i></b> Engines: DISABLED", gameObject.name), "Raimi", 3);
            NarrationWriter.Ambience(moodEdge.getMoods().relief);

            condition = shipCondition.adrift;
        }
    }
    


    private void ForwardThrust(float PowerToEngines) //pass percentage of throttle, -1 to 1
    {
        //ONLY TO BE CALLED IN FIXEDUPDATE
        
        //use REALTHROTTLE, but goal
        
        Mathf.Clamp(PowerToEngines, -1, 1);
        float PowerAvailable = EnginePowerAvailable();

        throttle = Mathf.Clamp(PowerToEngines, -PowerAvailable, PowerAvailable);
        float realThrust = mainEnginePower * GetComponent<Rigidbody>().mass;

        GetComponent<Rigidbody>().AddRelativeForce((Vector3.forward * realThrust * throttle), ForceMode.Force);
    }

    public void EvasiveThrust(float lateralThrust, float verticalThrust)
    {
        getRigidBody().AddRelativeForce(lateralThrust, verticalThrust, 0f, ForceMode.Force);

    }

    public void Pitch(float pitchControl, Space relativeDirection = Space.Self)
    {
        transform.Rotate(Vector3.right, turningPower * pitchControl * Time.deltaTime, relativeDirection);
    }

    public void Yaw(float yawControl, Space relativeDirection = Space.Self)
    {
        transform.Rotate(Vector3.up, turningPower * yawControl * Time.deltaTime, relativeDirection);
    }

    public void Roll(float rollOnZAxis, Space relativeDirection = Space.Self)
    {
        transform.Rotate(Vector3.forward, turningPower * rollOnZAxis * Time.deltaTime, relativeDirection);

    }

    public void manualControls(float mV, float mH, float hV, float hH, float th, float rX, float rY)
    {
        moveVertical = mV;
        moveHorizontal = mH;
        moveElev = hV;
        moveLateral = hH;
        throttle = th;
        rollx = rX;
        rolly = rY;
    }

    public void DeadStick()
    {
        manualControls(0, 0, 0, 0, 0, 0, 0);
    }

    // Free-flight mode
    void NormalFlight(bool engageFlight = true)
    {
        if (!engageFlight)
            return;

        //      STANDARD THRUST         /////////////////////////////////////////

        DynamicDrag();

        EvasiveThrust(moveLateral, moveElev);

        throttle = thrust;

        
        //      ROLL CONTROLS           /////////////////////////////////////////
       
        if (Mathf.Abs(rolly + rollx) > 0f)
        {
            //reset freeroll countdown
            rCTOcountdown = rollControlTimeOut;

        }

        //rotate player on z axis if engaged
        if (rCTOcountdown > 0f)
        {
            rCTOcountdown -= Time.deltaTime;
            freeRoll = true;
        }

        if (freeRoll)
            Roll(rollx);

        //  PITCH and YAW//////////////////////////////////////////////////////////

        Pitch(moveVertical);
        Yaw(moveHorizontal);

        // OVERBANK RETURN
        OverBankReturn(!freeRoll);
    }

    /*
    public void DeadStick()
    {
        moveLateral = 0f;
        moveElev = 0f;

        moveHorizontal = 0f;
        moveVertical = 0f;

        rollx = 0f;
        rolly = 0f;

        throttle = 0f;

    }*/


    void OverBankReturn(bool bankReset = true)
    {
        if (!bankReset)
            return;

        float overBanked = transform.localRotation.eulerAngles.z;

        if ((Mathf.Abs(overBanked) > overBankMargin && Mathf.Abs(moveHorizontal) < 0.2 && Mathf.Abs(moveVertical) < 0.2))
        {
            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0.0f, overBankCorrector * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newAngle);
        }
    }

    public bool WeaponsOnline()
    {
        //check for player or mob weapons

        return (weapons.Length > 0);
    }

    public void GravFlight()
    {
        //in-grav Controls
        if (ventralCruiseFlare != null) 
            ventralCruiseFlare.SetActive(getRigidBody().useGravity && Mathf.Abs(getVelocity()) > 10f);

        if (getRigidBody().useGravity)
            getRigidBody().AddForce(0.0f, hoverThrust, 0.0f);

    }

    public bool isThePlayer()
    {
        if  (pilot == shipController.player)
        {
            if (this == PlayerControls.getPlayerShip())
                return true;
        }
        return false;
    }

    void ConditionUpdate()
    {
        if (isThePlayer())
            pilot = shipController.player;
        

        switch(condition)
        {
            case shipCondition.waiting:
                if (eHP > 0)
                {
                    //if (throttle == 0)
                    condition = shipCondition.active;
                    //else
                    //   condition = shipCondition.anchored;
                }           
                else
                    condition = shipCondition.dead;
                break;
            case shipCondition.adrift:
                if (!WeaponsOnline())
                    beDisabled();
                break;
            case shipCondition.disabled:
                beDisabled();
                break;
            case shipCondition.dead:
                beDead();
                break;
            default: 
                condition = shipCondition.waiting;
                break;
        }
                      
    }

    public bool isShipActive()
    {
        return ((int)condition >= 10);
    }

    public bool isAlive()
    {
        //returns false if dead or disabled
        return ((int)condition >= 5);
    }

    public void beDisabled()
    {
        Color disabledColor = new Vector4(48.0f / 255.0f, 172.0f / 255.0f, 234.0f / 255.0f, 255.0f / 255.0f);

        condition = shipCondition.disabled;

        ShipDialogue(gameObject.name + ": DISABLED");

        if (disabledEffect != null)
        {
            disabledEffect.SetActive(true);
        }
    }

    public void beDead()
    {
        eHP = 0;

        soulsAlive = 0;
        condition = shipCondition.dead;
        if (deathEffect != null)
	        deathEffect.SetActive(true);
    }


    //public bool isFriendly(int iffCode)
    //{
    //    return false;
    //}

    public bool isHostile(LameShip mobToCheck)
    {
        return isHostile(mobToCheck.iff);
    }

    public bool isFriendly(LameShip mobToCheck)
    {
        return !isHostile(mobToCheck.iff);
    }
    

	public bool isFriendly(int iffCode){
		return !isHostile((mobAffiliation)iffCode);
	}

    public bool isHostile(mobAffiliation targetIff = mobAffiliation.Ally)
    {
        //checks if THIS MOB is hostile toward this affiliation, or in general against "good guys"
        int targetCategory = (int)targetIff / 10;

        int shipCategory = (int)iff / 10;

        if (targetCategory == 0)
            return false;

        if (targetCategory != shipCategory)
            return true;

        else
            return false;
    }

    public static bool areHostile(LameShip shipA, LameShip shipB)
    {
		 

        return shipA.isHostile(shipB);
    }

    public static bool areFriendly(LameShip shipA, LameShip shipB)
    {
        return !shipA.isHostile(shipB);
    }


    private void Awake()
    {
        getRigidBody();
    }

    public Rigidbody getRigidBody()
    {

        if (rb == null)
            rb = gameObject.GetComponent<Rigidbody>();

        //if still null, apply basic rigidbody
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();

            rb.mass = 1;

            rb.useGravity = false;
            rb.isKinematic = false;

            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        }


        return rb;
    }


    public void TurnTo(Vector3 direction)
    {
        direction.Normalize();
        getNavGyro().transform.forward = direction;
        TurnTo();
    }

    public void TurnTo(GameObject wayPoint = null)  //nav Gyro by default
    {

        //lock gyro to nav lcoation
        if (wayPoint != null)
            getNavGyro().transform.LookAt(wayPoint.transform);


        //lerp to match rotation

        //new heading
        float headingNav = navGyro.transform.rotation.eulerAngles.x;
        float markNav = navGyro.transform.rotation.eulerAngles.y;

        //original heading
        float heading0 = transform.rotation.eulerAngles.x;
        float mark0 = transform.rotation.eulerAngles.y;

        heading0 = Mathf.LerpAngle(heading0, headingNav, turningPower);
        mark0 = Mathf.LerpAngle(mark0, markNav, turningPower);

        Quaternion newcourse = Quaternion.Euler(new Vector3(heading0, mark0, 0.0f));

        transform.rotation = newcourse;

    }
    
    

    public void DynamicDrag(float forceNewDrag = -1f)
    {
        if (getRigidBody() == null)
            return;

        /*
        ////drag-free
        waiting = 0,
        dead = 1,
        disabled = 2,

        adrift = 4,

        

        //Rigidbody Frozen   
        anchored = 5,

        //Rigidbody  normal
        active = 10, //running: 10+
        
        takeover = 20
            */

        if ((int)condition > 5)
        {
            if (forceNewDrag < 0f)
            {
                rb.drag = normalDrag;
                rb.angularDrag = normalDrag;
            }

            else
            {
                rb.drag = forceNewDrag;
                rb.angularDrag = forceNewDrag;
            }
        }
        else if ((int)condition < 5)
        {
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }

        if (condition == shipCondition.anchored)
        {
            //come to a stop

            if (getVelocity() > 1)
                rb.drag = Mathf.MoveTowards(rb.drag, rb.drag + 1, Time.deltaTime);
            //cancel all motion  
            else
            {

               // rb.isKinematic = true;

                if (DockingPort != null)
                {

                    transform.position = DockingPort.transform.position;
                    transform.rotation = DockingPort.transform.rotation;
                }
            }
        }
        else
            rb.isKinematic = false;


    }
    
    public float getVelocity()
    {

        return getRigidBody().velocity.magnitude;
    }

    public void SetAlignment(int affil = 0)
    {
        SetAlignment((mobAffiliation)affil);
    }

    public string ShipStatus(bool debugMode = false)
    {
        string status = "";

        status += "ID:\t" + gameObject.name + '\n';
        status += "Hull:\t" + eHP.ToString() + " / " + hullHP.ToString() + '\n';
        status += "IFF:\t" + gameObject.tag + '\n';
        
        status += "Souls:\t" + soulsAlive.ToString() + " / " + soulsOnboard.ToString() + '\n';
        
        status += mobDialogue + '\n';

        return status;
    }
    
    public void SetAlignment(mobAffiliation affil = mobAffiliation.NonThreat)
    {     
        iff = affil;

        switch (iff)
        {
            case mobAffiliation.Ally:
                gameObject.tag = "Ally";
                ColorChange(ScoreKeeper.allyColor);
                break;

            case mobAffiliation.Threat:
                gameObject.tag = "Threat";
                ColorChange(ScoreKeeper.threatColor);
                break;

            default:
                gameObject.tag = "NonThreat";
                ColorChange(ScoreKeeper.allyColor);
                break;

        }    
    }

    public void ColorChange(Color newColor)
    {
        textureChangeOnTag viles = transform.GetComponentInChildren<textureChangeOnTag>();
        if (viles != null)
        {
            //viles.TextureChange(gameObject.GetComponent<EnemyShipModular>());
        }

        for (int i = 0; i < engines.Length; i++)
        {
            ParticleSystem[] engineFlares = engines[i].transform.GetComponentsInChildren<ParticleSystem>();
            {
                for (int ii = 0; ii < engineFlares.Length; ii++)
                {
                    ParticleSystem.MainModule flareMain = engineFlares[ii].main;
                    flareMain.startColor = newColor;
                }
            }
        }

    }

    public void restoreShip()
    {
        eHP = hullHP;
        condition = shipCondition.active;

    }


    public void modHP(float change)
    {
        if (eHP <= 0)
        {
            eHP = 0;
            beDead();
            return;
        }
        eHP += change;
    }

    public void BulletHit(BulletScript1 bullet)
    {
        Debug.Log(bullet.shooter.ToString() + " is using the old bullet Script.");
        BulletHit(bullet.getShooter(), bullet.damage, (int)bullet.shooterIff);
    }

    public void BulletHit(Ammunition bullet)
    {
        BulletHit(bullet.getShooter(), bullet.damage, (int)bullet.shooterIff);
    }
	
    public void BulletHit(GameObject shooter, float hitDamage, int iffCode)
    {
        int damage = Mathf.RoundToInt( hitDamage);
        //ignore self-bullets and ignore if dead
        if ((int)condition < 3 || shooter == gameObject)
            return;



        mobAffiliation iff = (mobAffiliation)iffCode;

        //IF ALIVE: take damage
        if ((int)condition > 2)
            modHP(-damage);


      /*  if (shieldsUp)
        {
            if (damage < shieldHP)
                shieldHP -= damage;
            else if (damage > shieldHP)
            {

                float shieldhit = damage - shieldHP;
                if (shieldHP < 1)
                    modHP(shieldhit);

            }
        }

        else if (!shieldsUp)
            playerHP -= damage;

        */

        // Wake on all bullets?
        //            if (!active)
        //                Invoke("WarmActive", 1);

        /*
        if (bullet.shooterIff == null)
        {
            Debug.Log(name + "hit by " + bullet.name + " with no shooter");
            return;
        }*/

        if (!isFriendly(iffCode))
            DialogueBox.tracking = gameObject;

        wasHit = true;  //something ought to clear this flag if it wants to do something



        //ALERT THE CONTROLLER
        /*
        if (!isFriendly(iffCode))
        {
            // ALERT THE CONTROLLER
            //addTarget(shooter, true);
            //RedAlert();
        }

        if (!tenacious && WeaponsActive())
        {
            if (threats.Length > 0)
                navigatingTo = threats[0];
        }
        */
    }

    public void ShipDialogue(string outMessage = "", float displayTime = 1f)
    {
        mobDialogue = outMessage;
        CancelInvoke("ClearDialogue");
        Invoke("ClearDialogue", displayTime);
    }

    public void ClearDialogue()
    {
        mobDialogue = "";
    }

    public void DockHere(GameObject dockObject)
    {
        //if docking, nav to here
        float range = Vector3.Distance(transform.position, dockObject.transform.position);

        DockingPort = dockObject;

        if (range > 100f)
        {


            TurnTo(dockObject);
            if (range > 1000f)
                throttle = 1;
            else
            {
                throttle = (range / 1000);
                //ForwardThrust(throttle);

            }
        }
        else
        {
            if (pilot == shipController.player)
                PlayerControls.GetActivePlayer().status = PlayerControls.PlayerCondition.Docked;

            TurnTo(dockObject.transform.forward);

        }

    }
    
    void OnCollisionExit(Collision collisionInfo)
    {
        LameShip ship = collisionInfo.gameObject.GetComponentInParent<LameShip>();
        //Just do not take damage if player is docked
        if (ship != null)
        {
            if (ship.DockingPort != null)
                modHP(Mathf.Round(collisionInfo.relativeVelocity.magnitude * CollisionDamage));
            //meh, docking to anything it's fine.
        }


    }

    /*
    public void setThrottle(float set)
    {
        throttle = set;


    }*/

    public void NavTo(GameObject destination)
    {
        //check distance

        //is it close? 

        //if yes, reached

        //else check for obstruction

        //is the path obstructed?

        //if yes, turn and cut throttle until clear

        //if no, turn toward waypoint

        //full throttle
    }
    
    public void FixedUpdate()
    {
        if (condition == shipCondition.active && pilot == shipController.player)
        {
            ForwardThrust(throttle);
        }

    

    }

}
