using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RangeFinderGeneral : MonoBehaviour {

	public string rangeFound;
	public Text viewFinder;
	public bool Threats;
	public bool Allies;
	public bool Salvage;
	public bool Bosses;
	public bool Docks;
	public bool Objectives;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//reset string
		rangeFound = "";

		if (Threats) 
		{
			RangeFinding("Threat");
		}
		if (Bosses) 
		{
			RangeFinding("Boss");
		}
		if (Objectives) 
		{
			RangeFinding("Objective");
		}
		if (Allies) 
		{
			RangeFinding("Ally");
		}
		if (Salvage) 
		{
			RangeFinding("Salvage");
		}
		
		if (Docks) 
		{
			RangeFinding("Dock");
		}

		viewFinder.text = rangeFound.ToString();



	}

	void RangeFinding(string scanTag)
	{
		//Find Player
		GameObject Player = GameObject.FindGameObjectWithTag ("Player");

		//scan for target
		GameObject[] scanObject = GameObject.FindGameObjectsWithTag (scanTag);
		
		//find range to target
		for (int i = 0; i < scanObject.Length; i++) 
		{
			rangeFound = rangeFound +  "Distance to " + scanObject[i].name +": " + Vector3.Distance (Player.transform.position, scanObject [i].transform.position) + "\n";
			
		}
	}


}
