using UnityEngine;
using System.Collections;

public class RadarPinger : MonoBehaviour {




	public float pingTime;
	public float pingFrequency = 3.0f;
	
	public GameObject threatPulse;
	public GameObject allyPulse;
	public GameObject bossPulse;
	public GameObject objectivePulse;
	
	public GameObject groundPlane;

	// Use this for initialization
	void Start () {
		pingTime = 0.0f;
        if (groundPlane == null)
            groundPlane = GameObject.FindGameObjectWithTag("GridFloor");
	}
	
	// Update is called once per frame
	void Update () {




		//incriment timer
		pingTime = pingTime + Time.deltaTime;

		if (pingTime > pingFrequency) 
		{
			//scan for each type
			Ping ("Threat", threatPulse);
			Ping ("Ally", allyPulse);
			Ping ("Boss", bossPulse);
			Ping ("Objective", objectivePulse);


			//reset ping timer
			pingTime = 0.0f;
		}

	
	}

	void Ping (string scanType, GameObject pingMarker)
	{

		//identify targets
		GameObject[] scannedTags = GameObject.FindGameObjectsWithTag (scanType);

		//for each enemy, instantiate a radar ping ever t seconds
        if (groundPlane != null)
        {
            foreach (GameObject bogey in scannedTags)
            {
                //generate pulse at location
                Instantiate(pingMarker, bogey.transform.position, groundPlane.transform.localRotation);
			
                //also generate on ground plane
			
                Vector3 hudreflect = new Vector3(bogey.transform.position.x, groundPlane.transform.position.y, bogey.transform.position.z);
                Instantiate(pingMarker, hudreflect, groundPlane.transform.localRotation);
			
			
            }
        }
        else if (groundPlane == null)
            groundPlane = GameObject.FindGameObjectWithTag("GridFloor");
        
	}

}
