using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour {

	public string shipClass;

	[Multiline(8)]
	public string shipLog;

	public enum ShipCondition {
		booting = 0,
		error = 1,
		dead = 2,
		disabled = 3,
		adrift = 4,

		//healthy > 4
		anchored = 5,
		active = 10,

	} public ShipCondition condition;

	public enum Affiliation
	{
		// GENRALLY HARMLESS 0-9
		// ALLIED MILITARY 10-19
		// RRELL OR OTHER THREATS 20-29

		NonThreat = 0,      // generic nonthreat
		Civillian = 1,

		Ally = 10,          // generic ally
		Raysian = 11,

		Threat = 20,        // generic threat
		Rrell = 21,
		Converted = 21

	} public Affiliation iff;

	public enum NavMode
	{
		idle,
		patrol,
		waypoint,
		combat
	} public NavMode guidance;

	//total number of people on board... dead or converted upon loss
	public int souls = 1;
	
	[Header("Specifications")]
	public float mainEnginePower = 1;
	public float evasivePower = 1;
	public float turningPower = 1;
	public float hoverThrust = 100f;


	public float maxSafeVelocity = 200;
	


	[Range(-.5f, 1f)]
	public float throttle = 0f;

	public float velocityReport = 0f;

	public float CollisionDamage = 10;

	// Gimbal and Navigation
	public float bankGimbalForce = -1.0f;
	public float pitchForce = -1.0f;
	private float _pitcher;
	private float _banker;

	[Header("Ship Components")]
	
	
	private WeaponModule[] _weapons;
	public GameObject weaponsKit;
	
	private GameObject _navGyro; //turn this to move the ship
	private GameObject _shipFrame;	//this contains all of the ship components

	private Rigidbody _rb;
	
	[Header("Effects")]

	public GameObject lowHPEffect;
	public GameObject deathEffect;
	public GameObject disabledEffect;
	
	
	[Header("Physics")]
	private Vector3 _lastVelocity;
	private Vector3 _deltaVelocity;
	public float normalDrag = 0.25f;
	public float normalADrag = 0.35f;
	
	public float gimbalResetTime = 3.0f;
	public float mass = 1.0f;

	public string mobDialogue = "";

	public bool wasHit;     //flag that shows ship was recently hit

	public Animator battleAnim;         //host of animator
	public List<GameObject> ReEntryBurn;
	public GameObject ventralCruiseFlare;
	
	public GameObject navDestination;
	public GameObject navFloatingWaypoint;

	public GameObject DockingPort;
	public List<ShipModule> all_modules = new List<ShipModule>();

	private void Update()
	{
		if (condition == ShipCondition.error)
			return;
			
		//to bootup
		else if (condition == ShipCondition.booting){
			GoFlight();
			return;
		}

		//NORMAL BEHAVIOR
		//		6.	Is there a target?


		//			a. If yes, Nav to Target
		//		7.	Is there a Nav Destination?
		//			a. if no, anchor ship
		//		8.	Cruise to Destination
		if (condition == ShipCondition.active)
		{

			// DRAG CONTROL
			float velocityIntent = throttle * maxSafeVelocity;
			if (Rb.velocity.magnitude < velocityIntent)
				Rb.AddRelativeForce(Vector3.forward * (EnginePower * throttle * mass));	
			
			velocityReport = Rb.velocity.magnitude;
		}
	}
	

	public bool isNeutralized()
	{
		//neutralized if dead or disabled
		return ((int)condition != 0 && (int)condition < 4);
	}
	
	
	public void Log(string entry)
	{
		string logLine = Time.time + "\t" + entry + "\n";
		shipLog += logLine;
		Debug.Log(shipName + " " + entry);
	}

	private void GoFlight() {
		//Only activate ship after all systems are online	

		//check hull
		//check engines
		//check weapons

		//check RigidBody
		if (Rb != null)
			Log("RB Physics: ONLINE");
		else
		{
			Log("RB Physics: MISSING");
			condition = ShipCondition.error;
		}


		//check hull

		if (HP > 0)
		{
			int integrity = 100 * HP / HPmax;

			Log("Hull Integrity Systems: ONLINE, " + integrity + "%");
		}
		else { 
			Log("Hull Integrity Systems: OFFLINE\nAbandon Ship!");
			condition = ShipCondition.dead;
			return;
		}
		

		//Check Engines

		if (EnginePower > 0){
		
			Log("Engines ONLINE: " + (EnginePower *100 )+ "%");

			condition = ShipCondition.active;
		}
		else
		{
			Log("Engines OFFLINE\nShip Adrift!");
			condition = ShipCondition.adrift;
		}

					


		//		4.	Check Weapons
		//			a. if offline, is ship adrift?
		//				i. if adrift, ship is disabled
		//				ii. else ship is neutralized

		//		5.	Ship is Active
		
	}

	public float EnginePower
	{
		get
		{
			float power = 0f;
			EngineModule[] mods = GetComponentsInChildren<EngineModule>();
			foreach (EngineModule mod in mods)
			{
				if (mod.Ship == null)
				{
					mod.Ship = this;
					this.all_modules.Add(mod);
					Log(mod.moduleMessage);
				}
				if (mod.Ship == this && mod.Online)
					power += (1.0f / mods.Length);
			}
			return power;
		}
	}

	public int HP{
		get
		{
			int _shipHP = 0;
			HullModule[] mods = GetComponentsInChildren<HullModule>();
			foreach (ShipModule mod in mods)
			{
				if (mod.Ship == null)
				{
					mod.Ship = this;
					Log(mod.moduleMessage);
					this.all_modules.Add(mod);
				}
				if (mod.Ship == this && mod.Online)
					_shipHP = mod.HP;
			}
			
			return _shipHP;
		}
	}

	public int HPmax
	{
		get
		{
			int _maxHP = 0;
			HullModule[] mods = GetComponentsInChildren<HullModule>();
			foreach (ShipModule mod in mods)
			{
				if (mod.Ship == null)
				{
					mod.Ship = this;
					Log(mod.moduleMessage);
					this.all_modules.Add(mod);
				}
				if (mod.Ship == this && mod.Online)
					_maxHP = mod.maxHP;
			}

			return _maxHP;
		}
	}

	public string shipName{
		get {
			return name;
		}
	}

	public string ShipNameFormal{
		get {
			string shipfullname = "";

			
			switch(iff){
				case (Affiliation.Raysian):
					shipfullname += "RMF ";
					break;
				case (Affiliation.Rrell):
					shipfullname += "Rrell ";
					break;
			}

			shipfullname += shipName;

			return shipfullname;
		}
	}

	public Rigidbody Rb{
		//remove or deactivate rigidbody if found below
		get{
			if (_rb == null)
				_rb = gameObject.GetComponent<Rigidbody>();
			//if still null, apply basic rigidbody
			if (_rb == null)
			{
				_rb = gameObject.AddComponent<Rigidbody>();

				_rb.mass = 1;

				_rb.useGravity = false;
				_rb.isKinematic = false;

				_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

			}
			return _rb;
		}
	}
	
	public GameObject Gyro{
		get{
			if (_navGyro == null)
			{
				_navGyro = new GameObject("navGyro Null");
				_navGyro.transform.position = transform.position;
				_navGyro.transform.SetParent(transform);
			}

			return _navGyro;
		}
	}

	//Ship Nav Info

	public float PitchIntensity
	{
		get{
			float PitchIntensity = Gyro.transform.localRotation.eulerAngles.x - transform.localRotation.eulerAngles.x;
			if (PitchIntensity > 180) 
			{
				PitchIntensity -= 360; 
			}
			return PitchIntensity;
		}
	}
	public float YawIntensity
	{
		get{
			float YRotForce = Gyro.transform.localRotation.eulerAngles.y - transform.localRotation.eulerAngles.y;
			if (YRotForce > 180) 
			{
				YRotForce -= 360; 
			}

			return YRotForce;
		}
	}

	public GameObject ShipFrame
	{
		// creates an empty gameObject that holds the ship and all of its components.
		//ALL HULL COMPONENTS WILL BE ADOPTED
		get	{
			if (_shipFrame == null)
			{
				_shipFrame = new GameObject("Ship Frame (Gimbal)");
				_shipFrame.transform.position = transform.position;
				_shipFrame.transform.SetParent(transform);
			}

			return _shipFrame;
		}
	}

	public void GimbalAction()
	{
		


		//For PLAYER USE?
		/*
		if (GetComponentInParent<SpaceShip>().pilot == SpaceShip.shipController.player)
        {
            if (PlayerControls.GetActivePlayer() != null)
            {
                if (PlayerControls.GetActivePlayer().status == PlayerControls.PlayerCondition.Active)
                {
                    deltaY = PlayerControls.GetActivePlayer().mH;

                    deltaX = PlayerControls.GetActivePlayer().mV;
                }
            }
        }
		*/
		
		//angle between gyro and parent

		_banker = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, YawIntensity * bankGimbalForce, gimbalResetTime * Time.deltaTime);

		_pitcher = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, PitchIntensity * pitchForce, gimbalResetTime * Time.deltaTime);

		transform.localRotation = Quaternion.Euler(_pitcher, 0.0f, _banker);

	}

}


/*
 * 
 * 
 * 
Modules:
	• Flight Control
		a. Manual Controls
			i. Free Flight
			ii. Rail Flight
		b. Autopilot (AI)
			• NavigateTo(waypoint)
			• if clear
		c. Deadstick
	• Flight
		○ Ship is actually always on automatic pilot
		○ Manual controls send immediate directional commands
		○ Markers
			• Gyro
				□ Points at current waypoint
				□ ship will align itself to this pointer
			• Waypoint
				□ current goal of navigation
			• Destination
				□ final goal of this navigation
				□ may be updated once reached
				□ may dock or go idle upon reaching
				□ clears if reached and not updated
		○ If on course:
			• Ship stays on a rail towards waypoint
			• Starfox-style
			• NavigateTo(Waypoint)
			• Manuever within rail to aim weapons
		○ Pitch()
		○ Yaw()
		○ Throttle()
			• check engine power
		○ Roll()
			§ Timeout()
			§ Correct()
		○ NavigateTo(Waypoint)
			§ Plot course
		○ plotCourse:
			§ move waypoint until:
				□ waypoint has unobstructed view of destination
				□ waypoint has unobstructed view of target
		
		
	• Fire Control
		○ Select Weapons
		○ Fire weapon at target
	• 
		
	• RigidBody
		○ Colliders
	• Thrusters
		○ EnginePower
			§ return engine functionality 
			
			§ usePower if in use
		○ Integrity
			§ Restore()
			§ Damage()
			§ HitEffect()
			§ OFFLINE Effect
			
		○ ThrusterEffect
			§ getColor from IFF
			§ OFFLINE Effect
			§ reflect thruster power (dot) thruster vector
		○ request power
		○ HitBox
		○ drag control
	• Hull / Shields
		○ Hull Integrity / HP
		○ Condition
			§ waiting
			§ dead
			§ disabled
			§ docked
			§ idle
			§ active
		○ Damage(value)
		○ Restore(value)
		○ getHP()
		○ getMax()
		○ getRatio()
		○ Inline compensatory shields
			§ Structural Integrity Field (generic)
				□ Bracer-Fields?
				□ "Bracers"
				□ Failproofing
			§ requests power
	• Power Hub 
		○ Cellular Zero-Point
		○ Power supply
			§ determines rate of recharge
		○ capacitance / charge
		○ request power
		○ power budget
			§ Handles power requests
				1) Engines
					a) points per second
				2) Weapons (on demand)
				3) Fold (on demand)
				4) Bracers
	• Affiliation
		○ Global Affiliation manager
		○ Friendly code
		○ Hostility
			§ static areHostile(checkA, checkB)
			§ local isHostile(check)
		○ Material Changer
		○ tag setter
		○ SetIFF
	• Weapons
		○ Ammunition
			§ damage
		○ Fire rate
		○ fire arc
		○ targetting pointer
		○ if in arc
			§ aim toward pointer
			§ fire
				□ wait for rate,
				□ repeat
		○ Stormtrooper
			§ scatter the target
		○ Charge cost
		
	
		

 * 
 * 
 * 
 */
