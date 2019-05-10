using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Raimi : MonoBehaviour {

    public bool active = false;
	public Transform playerFocus;
	public float thrustInit = 50;
	public float lowThrust = 5;
	public bool cruise = true;
	public float minVel = 1;
	public float thrust;
	public float slowRange = 10.0f;



    public float turning = .3f;
    private float throttle = 0f;
    public Vector3 navOffset = new Vector3(-100f, 200f, 0f);
    public float evasionDistance = 200f;

    public HUDTracker HUD;
	public Transform selected;

	public Canvas raimiHUD;
	public Text targetID;


    private GameObject navGyro;

    public static Raimi me;

    public GameObject navigatingTo;

    void Awake()
    {

        me = GetComponent<Raimi>();
        
    }

	void Start () {
        playerFocus = ScoreKeeper.playerAlive.transform;
        me = GetComponent<Raimi>();

        /*
        cruise = true;
		thrust = thrustInit;

        //get selection from Hud Tracker

        
		if (HUD.activeTarget != null) {

			selected = HUD.activeTarget.transform;
			targetID.text = selected.name.ToString ();
		}
        */

        if (navGyro == null)
        {
            navGyro = new GameObject("navGyro Null");
            navGyro.transform.position = transform.position;
            navGyro.transform.SetParent(transform);

        }
    }




    //
    //grab target based on touch input or mouse click
    //object must be tagged as a salvage item
    //change icon on mouse-over
    //fly to target
    //

    public static Reactions GetReactions()
    {
        return me.GetComponent<Reactions>();
    }

    void FixedUpdate()
    {
        /*
		if (HUD.activeTarget != null) {
			selected = HUD.activeTarget.transform;
			targetID.text = selected.name.ToString ();
		}
		if (cruise == true) 
		{
			Follower (playerFocus);

		}

		else if (cruise != true)

		{

			if (HUD.activeTarget != null) {

				Follower (selected);}
		}
	*/

        if (active)
        {

            if (navigatingTo == null)
            {
                navigatingTo = ScoreKeeper.playerAlive;
            }


            throttle = 100;

            if (!NavSteerAway(navigatingTo))
            {
                //if not avoiding an obstacle, head to waypoint

                TurnTo(navigatingTo);
            }


            Thrust(throttle);
        }
    }

	//receive clicks for context
	public void SickTarget ()
	{
	
		if (cruise == false) 
		{
			cruise = true;
		}

		else if (cruise == true) 
		{
			cruise = false;
		}
	}

	void Follower (Transform Target)
	{
		float range = Vector3.Distance (Target.position, transform.position);
		if (range < slowRange) 
		{
			thrust = lowThrust;
		} 
		else 
		{
			thrust = thrustInit;
		}

		GetComponent<Rigidbody>().AddRelativeForce (Vector3.forward * thrust);
		transform.LookAt (Target.position);

	}

    public void Thrust(float PowerToEngines = 100) //pass percentage of throttle
    {
        PowerToEngines = Mathf.Clamp(PowerToEngines, -100, 100);

        float realThrust = thrust * GetComponent<Rigidbody>().mass;

        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * realThrust * (PowerToEngines / 100), ForceMode.Force);
    }

    public void TurnTo(GameObject wayPoint = null)
    {
        if (wayPoint != null)
        {
                navGyro.transform.LookAt(wayPoint.transform.position + navOffset);
        }

        //lerp to match rotation

        //new heading
        float headingNav = navGyro.transform.rotation.eulerAngles.x;
        float markNav = navGyro.transform.rotation.eulerAngles.y;

        //original heading
        float heading0 = transform.rotation.eulerAngles.x;
        float mark0 = transform.rotation.eulerAngles.y;

        heading0 = Mathf.LerpAngle(heading0, headingNav, turning * Time.fixedDeltaTime);
        mark0 = Mathf.LerpAngle(mark0, markNav, turning * Time.fixedDeltaTime);

        Quaternion newcourse = Quaternion.Euler(new Vector3(heading0, mark0, 0.0f));

        transform.rotation = newcourse;

    }


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
        else
        {
            if (PathDetector.collider == waypoint && PathDetector.distance < evasionDistance)
            {
                throttle = 0;
            }
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

            //TURN RIGHT

            evadeMove = transform.right; // turn right to dodge
            

        }
        else
        {
            //if path ahead is clear, proceed forward until path to destination is clear

            evadeMove = transform.forward;

            //check if free to turn left, unturn
        }


        navGyro.transform.rotation = Quaternion.LookRotation(evadeMove, transform.up);
        TurnTo();

        return true;
    }


}
