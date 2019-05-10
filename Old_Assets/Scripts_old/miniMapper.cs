using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class miniMapper : MonoBehaviour {

#pragma warning disable 0169
    private GameObject[] totalScanned;
	public GameObject marker;
	public GameObject[] markers;
	public float scale = 50.0f;
	public Vector3 mapsize;

//	void Start()
//	{
//		mapsize = GameObject.FindGameObjectWithTag ("Boundary").transform.localScale;
//	}
//
//	// Update is called once per frame
//	void Update () {
//
//		//identify prefab markers, group
//		//total identifiable objects 
//
//		//adjust color for each tag object
//		totalScanned 	= GameObject.FindGameObjectsWithTag ("Threat");
//				
//		//assign a marker for each target
//		markers = GameObject.FindGameObjectsWithTag ("mapMarker");
//
//
//
//		//add markers
//		if (totalScanned.Length > markers.Length) 
//		{
//			for (int i = markers.Length; i < totalScanned.Length; i--)
//			{
//				Instantiate (marker);
//
//			}
//			markers = GameObject.FindGameObjectsWithTag ("mapMarker");
//		}
//
//
//		//remove extras
//		else if (totalScanned.Length < markers.Length) 
//		{
//			for (int i = markers.Length; i > totalScanned.Length; i--)
//			{
//				Destroy (markers [i - 1]);
//
//			}
//
//			markers = GameObject.FindGameObjectsWithTag ("mapMarker");
//		}
//
//
//
//		for (int i = 0; i < markers.Length; i++)
//		{
//
//			markers [i].transform.SetParent (transform);
//			markers [i].transform.localPosition = new Vector3 (scale * totalScanned [i].transform.position.x / mapsize.x, scale * totalScanned [i].transform.position.y / mapsize.y, 0.0f);
//
//
//		}
//
//	}


	/*
	void AssignMap(GameObject[] things, GameObject[] thingsHUD)
	{
		if (things.Length > 0)
		{


			for (int i = 0; i < things.Length; i++) 
			{

				//parent object to camera
				thingsHUD[i + 1].transform.parent = transform;

				//zero position
				thingsHUD[i + 1].transform.localPosition = Vector3.zero;

				//line up with object in view
				thingsHUD[i + 1].transform.LookAt (things[i].transform.position);


				//distance to target
				float rangeTarget = Vector3.Distance (things[i].transform.position, ScoreKeeper.playerAlive.transform.position);


				//find HUD renderer
				Renderer HUD = thingsHUD[i + 1].GetComponentInChildren<Renderer> ();



				thingsHUD[i + 1].transform.GetComponentInChildren<ThreatHP>().uiCanvas.enabled = true;
				ThreatHP statusBar =  thingsHUD[i + 1].transform.GetComponentInChildren<ThreatHP>();
				if (rangeTarget < sensorRange && things[i].GetComponent<EnemyShipModular>())
				{


					//display HPBar on hostile targets
					statusBar.hpbar.value = (things[i].GetComponent<EnemyShipModular>().eHP / things[i].GetComponent<EnemyShipModular>().maxHP);


					string enemyStatus = " ";

					if (things[i].tag == "Threat")
					{enemyStatus = "Hull: " + things[i].GetComponent<EnemyShipModular>().eHP + " / " + things[i].GetComponent<EnemyShipModular>().maxHP;
					}

					else if (things[i].tag == "NonThreat")
					{enemyStatus = "DISABLED: " + things[i].GetComponent<EnemyShipModular>().eHP + " / " + things[i].GetComponent<EnemyShipModular>().maxHP;
						Color disabledColor = new Vector4(48.0f/255.0f,172.0f/255.0f,234.0f/255.0f,255.0f/255.0f);
						statusBar.hpcolor.color = disabledColor;}

					else if (things[i].tag == "Objective")
					{
						enemyStatus = "OBJECTIVE: " + things[i].GetComponent<EnemyShipModular>().eHP + " / " + things[i].GetComponent<EnemyShipModular>().maxHP;
						Color objColor = new Vector4(0.0f / 255.0f, 255.0f / 255.0f, 33.0f / 255.0f, 202.0f / 255.0f);
						statusBar.hpcolor.color = objColor;
					}

					else if (things[i].tag == "Boss")
					{
						Color bossColor = new Vector4(255.0f/255.0f,0.0f/255.0f,237.0f/255.0f,255.0f/255.0f);
						enemyStatus = "BOSS: " + things[i].GetComponent<EnemyShipModular>().eHP + " / " + things[i].GetComponent<EnemyShipModular>().maxHP;
						statusBar.hpcolor.color = bossColor;
					}

					else if (things[i].tag == "Salvage") 
					{enemyStatus = "DESTROYED / SALVAGE";
						statusBar.hpbar.gameObject.SetActive(false);}



					//statusUpdate
					statusBar.hptxt.text = things[i].name + ": " + (Mathf.Round(rangeTarget) * rfScale) +"m" + "\n" + enemyStatus;


					//					if (things[i].GetComponent<EnemyShipModular>().battleMode == false || things[i].GetComponent<EnemyShipModular>().eHP <1.0f)
					//					{
					//						statusBar.hptxt.text = things[i].GetComponent<EnemyShipModular>().tag;
					//					}
				}
				else if (rangeTarget < sensorRange && things[i].tag == "Objective") 
				{	//statusUpdate
					statusBar.hptxt.text = things[i].name + ": " + (Mathf.Round(rangeTarget) * rfScale) +"m" + "\n" + "OBJECTIVE";


					statusBar.hpbar.gameObject.SetActive(false);
				}

				else 
				{
					thingsHUD[i + 1].transform.GetComponentInChildren<ThreatHP>().uiCanvas.enabled = false;
				}

				if (rangeTarget < MAXsensorRange) 
				{
					//enable renderer if in range
					HUD.enabled = true;

					//check if selectable

				} 

				else 
				{
					//disable renderer if not in range
					HUD.enabled = false;

				}
				//HUD.enabled = true;

			}
		}

	}

	void TagUpdate(GameObject[] things, GameObject[] thingsHUD)
	{
		//TAG UPDATE

		//spawn new HUDs

		//keep one original hud in place
		if (things.Length  + 1 > thingsHUD.Length) 
		{
			int diff = things.Length -  thingsHUD.Length + 1;

			while(diff > 0)
			{

				Instantiate(thingsHUD[0], hudMaster.transform.position, hudMaster.transform.rotation);

				diff--;			
			}
		}

		if (things.Length  + 1 < thingsHUD.Length) 
			//destroy extras
		{
			int diff = (thingsHUD.Length - 1) - things.Length;

			while(diff > 0)
			{
				int element = (thingsHUD.Length) - diff;
				Destroy(thingsHUD[element]);
				diff--;			
			}
		}
	}
	void UpdateTagged()
	{
		//update tagged object lists

		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		Dock = GameObject.FindGameObjectsWithTag ("Dock");
		allies = GameObject.FindGameObjectsWithTag ("Ally");
		threats = GameObject.FindGameObjectsWithTag ("Threat");
		junks = GameObject.FindGameObjectsWithTag ("Junk");
		salvages = GameObject.FindGameObjectsWithTag ("Salvage");
		bosses = GameObject.FindGameObjectsWithTag ("Boss");
		nonthreats = GameObject.FindGameObjectsWithTag ("NonThreat");
	}

	void UpdateHUD()
	{
		//update hud lists

		ObjHUD = GameObject.FindGameObjectsWithTag ("ObjHUD");
		DockHUD = GameObject.FindGameObjectsWithTag ("DockHUD");
		allyHUD = GameObject.FindGameObjectsWithTag ("AllyHUD");
		threatHUD = GameObject.FindGameObjectsWithTag ("ThreatHUD");
		junkHUD = GameObject.FindGameObjectsWithTag ("JunkHUD");
		salvageHUD = GameObject.FindGameObjectsWithTag ("SalHUD");
		bossHUD = GameObject.FindGameObjectsWithTag ("BossHUD");
		nonthreatHUD = GameObject.FindGameObjectsWithTag ("NonThreatHUD");
		activeHUD = GameObject.FindGameObjectWithTag ("ActiveHUD");

	}
	*/
}
