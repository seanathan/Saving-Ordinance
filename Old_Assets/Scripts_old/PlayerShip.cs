using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {

	//Agility/Power Settings
	public float agility = 1000.0f;
	public float thrustPower = 150.0f;
	public float bankSpeed = 100.0f;
	public float pitch = 1000.0f;
	public float yaw = 1000.0f;
	public float tilt = 1000.0f;

	public float dockBank = -750.0f;
	public float dockPitch = 1500.0f;
	public float dockElevate = 100.0f;
	public float dockThrust = 4000.0f;

	public float hullHP = 1000.0f;
    public float shieldHP = 500;

    public Transform[] hardPoints;

    public SetOfGameObjects ReEntryBurn;
    

	//Main Player Control
	public PlayerControllerAlpha Player;
    
	public GameObject ventralCruiseFlare;

	public GameObject shipModel;
	public GameObject cockpit;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerAlpha>();
    }


	void Update()
	{
        /*
        if (!Player.activeChassis)
		{
			Player.activeChassis = gameObject.GetComponent<PlayerShip>();		
		}	
        */

        if (Player && ReEntryBurn)
            ReEntryBurn.Activate(Player.ReEntry);


	}
}
