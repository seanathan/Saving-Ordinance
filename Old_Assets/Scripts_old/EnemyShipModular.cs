using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyShipModular : MonoBehaviour {


    public enum mobCondition
    {
        //drag-free
        waiting = 0,
        dead = 1,
        disabled = 2,

        adrift = 4,



        //Rigidbody Frozen   
        anchored = 5,

        //Rigidbody  normal
        active = 10, //running: 10+

        takeover = 20
    }

    public mobCondition condition;

    public enum combatMode
    {
        peaceful = 0,
        fighting = 1,

        patrolling = 10,
        engaging = 11,

        pursuit = 20,
        assault = 21,

        escorting = 30,
        defending = 31

        // %10 = 0 = noncombat
        // %10 = 1 = inCombat
    }

    public combatMode combat;


    public enum mobAffiliation
    {
        NonThreat = 0, //Harmless: 0s
        Civillian = 1,
        //Disabled =1,
        //Salvage = 2,
        //Objective = 3,
        Ally = 10,  //allies 10s
        Raysian = 11,


        Threat = 20,  // threat 20s
        Rrell = 21
        //Boss = 21,
    }


    public mobAffiliation iff;

    public enum mobPriority
    {
        normal,
        objective,
        boss
    }

    public mobPriority priority;

    //public bool active;

    [Range(-50, 100)]
    public int throttleOverride = 0;
    public float velocity = 0;



    [Header("Power Variables")]


    public float engineThrust = 100.0f; // factor of rigidbody mass
    public float evasiveThrust = 1000f;
    public float evasionDistance = 100f;
    public float kTurning = 0.3f;       //between 0 and 1, rate of turning
    public float eHP;                   //Reflects Current Value of HP
    public float maxHP;                 //Maximum Value of HP (Set this)
    public int scorePoints;             //Points possible for defeating ship
    public int collDamage = 200;        //Damage if colliding with object
    public int attackRange = 2000;      //Maximum Firing Range
    public float stormTrooper = 1.0f;   //Accuracy of Weapons... if 0, weapons will be spot on. 1 will apply "normal" inaccuracy levels.
    private float normalDrag;
    private float normalADrag;


    [Header("Ship Modules")]
    public EnemyEngineModule[] engines;   //Container for ship Engines
    public GameObject navGyro;             //Navigation Pointer

    public EnemyGunTurret[] weapon;     //Current List of Weapons on Board

    public GameObject winDrop;          //Prefab to drop if Defeated
    private bool drop = true;           //WinDrop will Drop when conditions are met

    public GameObject splode;           //Death effect
    public GameObject disSplode;        //Disable effect

    [Header("Breakable Hull parts (up to 2)")]
    public bool breakableHull;
    public GameObject breakoff0;
    public GameObject breakoff1;

    public Animator battleAnim;         //host of animator

    //	private bool pauseMotion = false;

    public int soulsOnboard;
    public int soulsAlive;
    //    public bool Allied = false;
    public bool Objective = false;
    public bool restore = false;
    public bool reset = false;

    public bool battleMode = false;

    [Header("will set direct course for target")]
    public bool assaultMode = false;
    public GameObject assaultTarget;

    //Escort mode: Will throw battlemode if their escort is hit
    //No action if empty
    public EnemyShipModular escorting;
    public bool Distress = false;
    public float distressRange = 0f; // if 0, will alarm entire map

    [Header("Will attack target when in range")]
    public bool aggressive = false;

    [Header("will attack player but will stay on  course")]
    public bool tenacious = false;
    public GameObject activeTarget;
    public int maximumTargets = 1;
    private int threatCount = 0;
    public GameObject[] threats;

    public bool targetPlayerByDefault = false;

    [Header("Navigation and Behavior")]
    public GameObject navigatingTo;
    public GameObject nextDestination;
    public Vector3 navOffset = new Vector3(-100f, 200f, 0f);

    public bool dying = false;
    public bool disabled = false;

    [HideInInspector] // other things we don't need to control

    //  public bool enginesDisabledFlag = false;
    //   [HideInInspector]
    public float throttle = 100f;       // Default at 100% throttle

    public string mobDialogue = "";

    public bool takenover = false;
    public bool testDisable = false;

    void Awake()
    {
        normalDrag = GetComponent<Rigidbody>().drag;

        normalADrag = GetComponent<Rigidbody>().angularDrag;

        engines = GetComponentsInChildren<EnemyEngineModule>();

        AllClear();
    }


    void ConditionUpdate()
    {
        if (condition == mobCondition.waiting)
        {
            if (eHP > 0)
            {
                if (navigatingTo)
                    condition = mobCondition.active;
                else
                    condition = mobCondition.anchored;
            }

            else
                condition = mobCondition.dead;


        }


        if (condition == mobCondition.adrift)
            if (!WeaponsActive())
                Disabled();

        if (condition == mobCondition.disabled)
            beDisabled();

        if (condition == mobCondition.dead)
            beDead();


    }

    void DynamicDrag()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        velocity = rb.velocity.magnitude;


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
            rb.drag = normalDrag;
            rb.angularDrag = normalADrag;
        }
        else if ((int)condition < 5)
        {
            rb.drag = 0.01f;
            rb.angularDrag = 0.01f;
        }

        if (condition == mobCondition.anchored)
        {
            //come to a stop

            if (velocity > 1)
                rb.drag = Mathf.MoveTowards(rb.drag, rb.drag + 1, Time.deltaTime);
            //cancel all motion  
            else
                rb.isKinematic = true;
        }

        else
            rb.isKinematic = false;


    }


    public void SetAlignment(int affil = 0)
    {
        SetAlignment((mobAffiliation)affil);
    }


    public void SetAlignment(mobAffiliation affil = mobAffiliation.NonThreat)
    {

        iff = affil;


        if (iff == mobAffiliation.Ally)
        {
            gameObject.tag = "Ally";
            EngineChange(ScoreKeeper.allyColor);
        }

        if (iff == mobAffiliation.NonThreat)
        {
            gameObject.tag = "NonThreat";
            //  EngineChange(ScoreKeeper.allyColor);
        }

        if (iff == mobAffiliation.Threat)
        {

            gameObject.tag = "NonThreat";
            EngineChange(ScoreKeeper.threatColor);
        }
        //NonThreat = 0, //Harmless: 0s
        //Ally = 10,  //allies 10s
        //Threat = 20,  // threat 20s


        textureChangeOnTag viles = transform.GetComponentInChildren<textureChangeOnTag>();
        if (viles != null)
        {
            viles.TextureChange(gameObject.GetComponent<EnemyShipModular>());
        }


    }


    void Start()
    {
        if (navGyro == null)
        {
            navGyro = new GameObject("navGyro Null");
            navGyro.transform.position = transform.position;
            navGyro.transform.SetParent(transform);

        }

        AllClear();

        engines = GetComponentsInChildren<EnemyEngineModule>();

        ScoreKeeper.soulsMax += soulsOnboard;
        soulsAlive = soulsOnboard;

    }

    public bool isFriendly(mobAffiliation requestor = mobAffiliation.Ally)
    {
        if (iff == mobAffiliation.NonThreat)
            return true;

        //returns true if same broadly the same affiliation

        //neutrals 0-9, 
        //allies 10-19
        //enemies 20-30

        bool friendly = ((int)requestor / 10 == (int)iff / 10);

        return friendly;
    }

    void EnginesDisabled()
    {
        if (condition == mobCondition.active)
        {

            ShipDialogue(gameObject.name + " Engines: DISABLED");

            //Raimi Commentary
            NarrationWriter.PopDialogue(string.Format("<b><i>{0}</i></b> Engines: DISABLED", gameObject.name), "Raimi", 3);
            NarrationWriter.Ambience(moodEdge.getMoods().relief);

            condition = mobCondition.adrift;
        }
    }

    float EnginePower()
    {
        if (engines.Length == 0)
            return 1;

        float powerAvailable = 0;

        for (int i = 0; i < engines.Length; i++)
        {
            powerAvailable += engines[i].enginePower / engines.Length;
        }

        if (powerAvailable == 0)
            EnginesDisabled();

        return powerAvailable;
    }

    void FixedUpdate()
    {

        if (restore)
        {
            RestoreShip();
        }

        if (testDisable)
        {
            Disabled();
            testDisable = false;
        }

        if (Distress)
        {
            DistressCall();
            //visual distress signal
        }

        ConditionUpdate();

        DynamicDrag();

        //if (dying || disabled)
        //            active = false;

        if ((int)condition < 4)    //ship is inactive
            return;

        //// ACTIVE BEHAVIOR

        if (targetPlayerByDefault && assaultTarget == null)
        {
            assaultTarget = ScoreKeeper.playerAlive;
            assaultMode = true;
        }

        if (WeaponsActive())
            EngagementsUpdate();

        throttle = 0f; //start throttle at 0% for this frame

        //UPDATED
        if ((int)condition >= 10)    // active and enginesOn
        {
            throttle = 100;

            if (!NavSteerAway(navigatingTo))
            {
                //if not avoiding an obstacle, head to waypoint

                TurnTo(navigatingTo);
            }
            /*

            //kill thrust if evading!!
            if (NavEvasion())
                throttle = -30;

            */

        }

        //FIRING WEAPONS
        if ((int)combat % 10 == 1)
        {

            battleMode = true;

            if (WeaponsActive() && threats.Length > 0)
            {
                FireWeapons();
            }
            else
            {
                AllClear();
            }
        }
        else if (takenover)
            navigatingTo = null;
        else
        {
            navigatingTo = nextDestination;

            //rest all weapons
            for (int i = 0; i < weapon.Length; i++)
            {
                if (weapon[i] != null)
                    weapon[i].Rest();
            }
        }


        if (battleAnim != null)
        {
            // if (battleMode && !(threats[0] == "Objective" || threats[0].tag == "Boss"))

            if (battleMode)
            {
                battleAnim.SetBool("Battle", true);
            }
            else
            {
                battleAnim.SetBool("Battle", false);
            }
        }

        //if escorting
        if (escorting != null)
        {
            if (escorting.battleMode == true && Alive(escorting.gameObject))
            {
                if (escorting.threats.Length > 0)
                {
                    assaultTarget = escorting.threats[0];
                    foreach (GameObject bad in escorting.threats)
                    {
                        if (bad != null)
                            if (Vector3.Distance(bad.transform.position, transform.position) < Vector3.Distance(assaultTarget.transform.position, transform.position))
                                assaultTarget = bad;
                    }
                    assaultMode = true;
                    aggressive = true;
                }
            }
            else
            {
                assaultMode = false;
                assaultTarget = null;
            }
        }

        if (assaultMode && assaultTarget != null)
        {
            float assaultrange = Vector3.Distance(transform.position, assaultTarget.transform.position);
            addTarget(assaultTarget, assaultrange < attackRange);

            aggressive = true;

            navigatingTo = assaultTarget;


            //   addTarget(assaultTarget, Vector3.Distance(transform.position, assaultTarget.transform.position) < attackRange);

            //exit assault on target death / disable
            if (!Alive(assaultTarget.gameObject))
            {
                assaultTarget = null;
                assaultMode = false;
                aggressive = false;
            }
        }
        else
        {
            assaultMode = false;
        }

        //LAST STEP OF ACTIVE TURN

        if (throttleOverride != 0)
            throttle = throttleOverride;



        //Engage Thrust after calculations;
        Thrust(throttle);


        if (eHP <= 0)
        {
            if (dying == false)
                Death();

            eHP = 0;
        }
    }

    public void SaveShipCurrentStatus()
    {
        PlayerPrefs.SetInt(gameObject.name.ToString() + "alignment", (int)iff);

        PlayerPrefs.SetInt(gameObject.name.ToString() + "condition", (int)condition);

        /*
        if (Allied)
            PlayerPrefs.SetString(gameObject.name.ToString() + "alignment", "Ally");
        else if (Objective)
            PlayerPrefs.SetString(gameObject.name.ToString() + "alignment", "Objective");
        else
            PlayerPrefs.SetString(gameObject.name.ToString() + "alignment", "Threat");
            */


        SaveShipSpot(transform.position.x, transform.position.y, transform.position.z, transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    void SaveShipSpot(float px, float py, float pz, float rx, float ry, float rz, float rw)
    {
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "x", px);
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "y", py);
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "z", pz);
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "rx", rx);
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "ry", ry);
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "rz", rz);
        PlayerPrefs.SetFloat(gameObject.name.ToString() + "rw", rw);
    }

    bool Alive(GameObject mob)
    {

        return ((int)condition > 2);
        //  return (mob.tag != "Salvage" && mob.tag != "NonThreat");
    }

    bool WeaponsActive()
    {

        int activeWeapons = 0;

        weapon = transform.GetComponentsInChildren<EnemyGunTurret>(false);

        for (int i = 0; i < weapon.Length; i++)
        {
            if (!weapon[i].Special)
                activeWeapons++;
        }
        return (activeWeapons > 0);
    }

    public void RestoreShip()
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat(gameObject.name.ToString() + "x"), PlayerPrefs.GetFloat(gameObject.name.ToString() + "y"), PlayerPrefs.GetFloat(gameObject.name.ToString() + "z"));
        transform.rotation = new Quaternion(PlayerPrefs.GetFloat(gameObject.name.ToString() + "rx"), PlayerPrefs.GetFloat(gameObject.name.ToString() + "ry"),
            PlayerPrefs.GetFloat(gameObject.name.ToString() + "rz"), PlayerPrefs.GetFloat(gameObject.name.ToString() + "rw"));

        int recallTag = PlayerPrefs.GetInt(gameObject.name.ToString() + "alignment");
        SetAlignment(recallTag);


        //restoreShipToLife
        //condition = mobCondition.waiting;

        condition = (mobCondition)PlayerPrefs.GetInt(gameObject.name.ToString() + "Condition");

        if (condition == mobCondition.dead)
        {

            //restore as dead

            eHP = 0;

            splode.SetActive(true);

            return;
        }

        dying = false;
        disabled = false;

        eHP = maxHP;

        if (splode)
            splode.SetActive(false);

        if (disSplode)
            disSplode.SetActive(false);

        navigatingTo = nextDestination;

        restore = false;

        for (int i = 0; i < engines.Length; i++)
        {
            engines[i].RestoreEngines();
        }

        //restore all guns
        EnemyWeaponsModule[] armament = transform.GetComponentsInChildren<EnemyWeaponsModule>(true);
        if (armament.Length > 0)
        {
            for (int i = 0; i < armament.Length; i++)
            {
                armament[i].RestoreGun();
            }
        }

        if (reset)
        {
            SaveShipSpot(0, 0, 0, 0, 0, 0, 0);
            reset = false;
            //PlayerPrefs.SetInt(gameObject.name.ToString() + "alignment", 0);

        }


    }


    //experimental steering

    bool NavSteerAway(GameObject waypoint = null)
    {
        if (waypoint == null)
            return false;   //don't evade if not heading anywhere.

        //ray cast to nav waypoint, return false if clear

        Rigidbody myshiprb = GetComponent<Rigidbody>();

        Vector3 directionToWaypoint = (waypoint.transform.position - transform.position).normalized;

        // Ray pathToWaypoint = new Ray(transform.position, (waypoint.transform.position - transform.position).normalized);

        RaycastHit PathDetector;
        if (!(myshiprb.SweepTest(directionToWaypoint, out PathDetector, evasionDistance)))
        {

            //exits and returns false if nothing in the path
            return false;
        }

        //Debug.Log(gameObject.name.ToString() + ", Way is blocked");
        //continues if evading
        Vector3 evadeMove = Vector3.zero;


        RaycastHit EvadeDetector;

        //if (Physics.BoxCast(transform.position, 50*Vector3.one, transform.forward, Quaternion.Euler(Vector3.zero), evasionDistance))
        if (myshiprb.SweepTest(transform.forward, out EvadeDetector, evasionDistance))
        {
            //if path forward is obstructed, TURN

            //kill engines till clear
            throttle = 0;

            //TURN RIGHT

            evadeMove = transform.right; // turn right to dodge


        }
        else
        {
            //if path ahead is clear, proceed forward until path to destination is clear

            evadeMove = transform.forward;

            throttle = 100;

            //if (Physics.Raycast(transform.position, transform.forward, evasionDistance))


            //check if free to turn left, unturn
        }


        navGyro.transform.rotation = Quaternion.LookRotation(evadeMove, transform.up);
        TurnTo();

        return true;
    }


    bool NavEvasion()
    {
        Vector3 evadeMove = Vector3.zero;

        RaycastHit Detector;
        if (Physics.Raycast(transform.position, transform.forward, out Detector, evasionDistance))
        {
            //exclude self from 
            //if (Detector.rigidbody == null)
            //   return false;


            Debug.Log(gameObject.name.ToString() + ", TAKE EVASIVE ACTION" + Detector.collider.name);

            //check clearance for half 1/2 distance

            if (!(Physics.Raycast(transform.position, transform.right, evasionDistance / 2)))
            {
                evadeMove = Vector3.right;
            }

            else
            if (!(Physics.Raycast(transform.position, transform.up, evasionDistance / 2)))
            {
                evadeMove = Vector3.up;
            }

            else
            if (!(Physics.Raycast(transform.position, -transform.right, evasionDistance / 2)))
            {
                evadeMove = Vector3.left;
            }

            else
            if (!(Physics.Raycast(transform.position, -transform.up, evasionDistance / 2)))
            {
                evadeMove = Vector3.down;
            }

            Rigidbody myshiprb = GetComponent<Rigidbody>();
            myshiprb.AddRelativeForce(evadeMove * (evasiveThrust * myshiprb.mass) * Time.fixedDeltaTime, ForceMode.Force);

            return true;
        }

        return false;
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

    public string ShipStatus(bool debugMode = false)
    {
        string status = "";

        status += "ID:\t" + gameObject.name + '\n';
        status += "Hull:\t" + eHP.ToString() + " / " + maxHP.ToString() + '\n';
        status += "IFF:\t" + gameObject.tag + '\n';

        if (debugMode)
        {
            if (navigatingTo != null)
                status += "Nav To: \t" + navigatingTo.name.ToString() + '\n';
            else
                status += "Nav To: \t INACTIVE" + '\n';
            status += "Souls:\t" + soulsAlive.ToString() + " / " + soulsOnboard.ToString() + '\n';

        }
        status += mobDialogue + '\n';

        return status;
    }

    void FireWeapons()
    {
        if (threats.Length == 0)
        {
            Debug.Log("nothing to shoot at!");
            return;
        }

        for (int i = 0; i < weapon.Length; i++)
        {
            if (weapon[i].Special)
            {
                if (threats[0].tag == "Objective" || threats[0].tag == "Boss")
                    weapon[i].Firing(threats[0], stormTrooper);
                else
                    weapon[i].Rest();
            }

            else if (i < threats.Length)
            {
                if (threats[i] != null)
                    weapon[i].Firing(threats[i], stormTrooper);
                else
                    weapon[i].Firing(threats[0], stormTrooper);
            }
            else
                weapon[i].Firing(threats[0], stormTrooper);

        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {

        //Just do not take damage if player is docked
        // if ((PlayerControllerAlpha.GetActivePlayer().status != PlayerControllerAlpha.PlayerCondition.Docked) && (Vector3.Distance(collisionInfo.collider.transform.position, transform.position) < 1000f))
        if ((PlayerControls.GetActivePlayer().status != PlayerControls.PlayerCondition.Docked) && (Vector3.Distance(collisionInfo.collider.transform.position, transform.position) < 1000f))

        {

            eHP -= collDamage;
        }


    }

    void beDead()
    {
        eHP = 0;

        soulsAlive = 0;
        dying = true;

        condition = mobCondition.dead;

        splode.SetActive(true);
    }

    void Death()
    {
        AllClear();


        ShipDialogue(gameObject.name + " destroyed. All " + soulsOnboard + " souls lost.");
        //       DialogueBox.PrintToDBox(gameObject.name + " destroyed. All " + soulsOnboard + " souls lost.", gameObject);

        if (soulsOnboard > 10)
        {


            if (!isFriendly())
            {
                DialogueBox.enemyBar.gameObject.SetActive(false);
                DialogueBox.enemyStatus.text = "";
                DialogueBox.tracking = null;

                Raimi.GetReactions().Remorse(string.Format("<b><i>{0}</i></b> has been destroyed... Objective complete, though all <i>{1} souls</i> have been lost.", gameObject.name, soulsAlive));
            }

            else
                Raimi.GetReactions().Loss(string.Format("We've lost the <b><i>{0}</i></b>. All <i>{1} crew</i> aboard are being reported as casualties.", gameObject.name, soulsOnboard));
        }
        else
            NarrationWriter.PopDialogue(string.Format("<b><i>{0}</i></b> has been destroyed.", gameObject.name), "Raimi", 3);

        beDead();


        if (drop == true && winDrop != null)
        {
            drop = false;
            Instantiate(winDrop, transform.position, transform.rotation);
        }


        if (splode != null)
            splode.SetActive(true);
    }

    void beDisabled()
    {
        Color disabledColor = new Vector4(48.0f / 255.0f, 172.0f / 255.0f, 234.0f / 255.0f, 255.0f / 255.0f);
        //    gameObject.tag = "NonThreat";
        //	fillBar.color = disabledColor;

        //   enginesDisabledFlag = true;

        //   iff = mobAffiliation.NonThreat;
        //active = false;

        condition = mobCondition.disabled;

        ShipDialogue(gameObject.name + ": DISABLED");

        if (disSplode != null)
        {
            disSplode.SetActive(true);
        }


    }

    void Disabled()
    {
        beDisabled();

        AllClear();

        //Raimi Commentary
        Raimi.GetReactions().Fiero(string.Format("<b><i>{0}</i></b> has been disabled! Minimal Casualties", gameObject.name));

        //disabled = true;

        //        DialogueBox.PrintToDBox(gameObject.name + ": DISABLED", gameObject);

        if (drop == true && winDrop != null)
        {
            Instantiate(winDrop, transform.position, transform.rotation);
            drop = false;
        }




    }

    public void Thrust(float PowerToEngines = 100) //pass percentage of throttle
    {
        float PowerAvailable = EnginePower() * 100;

        PowerToEngines = Mathf.Clamp(PowerToEngines, -PowerAvailable, PowerAvailable);

        float realThrust = engineThrust * GetComponent<Rigidbody>().mass;

        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * realThrust * (PowerToEngines / 100), ForceMode.Force);



    }

    public void TurnTo(GameObject wayPoint = null)
    {
        if (wayPoint != null)
        {
            //lock navGyro to nav lcoation if given
            if (!battleMode)
                navGyro.transform.LookAt(wayPoint.transform);
            else
                navGyro.transform.LookAt(wayPoint.transform.position + navOffset);
        }

        //lerp to match rotation

        //new heading
        float headingNav = navGyro.transform.rotation.eulerAngles.x;
        float markNav = navGyro.transform.rotation.eulerAngles.y;

        //original heading
        float heading0 = transform.rotation.eulerAngles.x;
        float mark0 = transform.rotation.eulerAngles.y;

        heading0 = Mathf.LerpAngle(heading0, headingNav, kTurning * Time.fixedDeltaTime);
        mark0 = Mathf.LerpAngle(mark0, markNav, kTurning * Time.fixedDeltaTime);

        Quaternion newcourse = Quaternion.Euler(new Vector3(heading0, mark0, 0.0f));

        transform.rotation = newcourse;

    }

    public void DistressCall()
    {
        EnemyShipModular[] AllShips = FindObjectsOfType<EnemyShipModular>();
        foreach (EnemyShipModular ship in AllShips)
        {
            /*
            if (ship.tag == gameObject.tag)
                ship.escorting = gameObject.GetComponent<EnemyShipModular>();
                */

            string me = gameObject.tag;
            string them = ship.tag;

            //Distress range 0 will summon everyone!

            if (distressRange == 0 || Vector3.Distance(transform.position, ship.transform.position) < distressRange)
            {
                if (ship.isFriendly(iff))
                    ship.escorting = gameObject.GetComponent<EnemyShipModular>();
            }
        }

    }

    public void BulletHit(BulletScript1 bullet)
    {
        Debug.Log(bullet.shooter.ToString() + " is using the old bullet Script.");
        BulletHit(bullet.getShooter(), bullet.damage, bullet.shooterIff);
    }

    public void BulletHit(Ammunition bullet)
    {
        BulletHit(bullet.getShooter(), bullet.damage, bullet.shooterIff);
    }

    public void BulletHit(GameObject shooter, float damage, EnemyShipModular.mobAffiliation iff)
    {
        if ((int)condition < 3 || shooter == gameObject)
            return;
        

        //IF ALIVE: take damage
        if ((int)condition > 2)
            eHP -= damage;
        
        // Wake on all bullets?
        //            if (!active)
        //                Invoke("WarmActive", 1);

        /*
        if (bullet.shooterIff == null)
        {
            Debug.Log(name + "hit by " + bullet.name + " with no shooter");
            return;
        }*/

        if (!isFriendly())
            DialogueBox.tracking = gameObject;
        
        if (!isFriendly(iff))
        {
            addTarget(shooter, true);
            RedAlert();
        }
            
        if (!tenacious && WeaponsActive())
        {
            if (threats.Length > 0)
                navigatingTo = threats[0];
        }

    }
    
    public void RedAlert()
    {
        if ((int)condition > 2)
        {
            if ((int)condition < 10)
                Invoke("WarmActive", 1);

            if ((int)combat % 10 == 0)
            {
                ShipDialogue(gameObject.name + " aggravated");

                //instantiate big red alarm flash

                if (GetComponentInChildren<RedAlert>() != null)
                    GetComponentInChildren<RedAlert>().RedAlarm();

                Invoke("WarmBattle", 1);

                //Single Distress call
                DistressCall();
            }
        }
    }

    void WarmActive()
    {

        condition = mobCondition.active;
        //active = true;
    }

    void WarmBattle()
    {

        if ((int)combat % 10 == 0)
        {

            Debug.Log(name + " Aggravated");

            combat++;
        }
        if (!battleMode)
        {
            battleMode = true;
        }
    }

    public void AllClear()
    {
        battleMode = false;
        if ((int)combat % 10 == 1)
            combat--;

        threats = new GameObject[4] { null, null, null, null };

        navigatingTo = nextDestination;
    }
    
    public void EngagementsUpdate()
    {
        if (threats.Length > 0)
        {
            foreach (GameObject target in threats)
            {
                if (target != null)
                {
                    float assaultrange = Vector3.Distance(transform.position, target.transform.position);
                    addTarget(target, (assaultrange < attackRange && Alive(target)));
                }
            }
        }
        else if (threats.Length == 0)
            AllClear();

    }


    public void addTarget(GameObject mob, bool add = true)
    {
        //Adds by default, reverse flag for removal;

        if (mob == null)
            return;

        GameObject[] oldList = threats;

        //check if target is in list
        for (int i = 0; i < threats.Length; i++)
        {
            if (threats[i] == mob)
            {
                if (add)
                    return;
                else
                {
                    //remove association from list
                    threats[i] = null;
                }
            }

            //do not keep empty slots
            if (threats[i] == null)
                threatCount--;
        }
        if (add)
            threatCount++;

        //error catch
        if (threatCount < 0)
        {
            threatCount = 0;
            Debug.Log("Too many threats removed");
        }

        //new list with no blank spots

        threats = new GameObject[threatCount];

        int j = 0;
        int k = 0;
        while (j < threatCount && k < oldList.Length)
        {

            //do not keep if blank

            if (oldList[k] == null)
                k++;
            else
            {
                threats[j] = oldList[k];
                j++;
                k++;
            }
        }

        if (add)
        {
            RedAlert();
        }
    }

    
    
    void OnTriggerEnter(Collider other)
	{

        if (other.gameObject == nextDestination) 
		{
			navWay incoming = other.gameObject.GetComponent<navWay>();

			nextDestination = incoming.nextNav;
		}
	}

    public void EngineChange(Color newColor)
    {
        for (int i = 0; i < engines.Length; i++)
        {
            ParticleSystem[] engineFlares = engines[i].transform.GetComponentsInChildren<ParticleSystem>();
            {
                for (int ii = 0; ii< engineFlares.Length; ii++)
                    engineFlares[ii].startColor = newColor;
            } 
        }
    }
}