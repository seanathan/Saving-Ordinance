using UnityEngine;
using System.Collections;

public class objectiveGoals : MonoBehaviour {
    public bool NeutralizeAllThreats = false;
    public int ObjectiveShipsTotal = 2;
    public int ObjectiveShipsMinimum = 0;
    public bool failToProtect = false;

	// Update is called once per frame
	void Update () {
        NeutralizeAllThreats = NeutralizeAll();
        failToProtect = FailedObjectiveShips(ObjectiveShipsMinimum);

    }

    bool NeutralizeAll()
    {
        GameObject[] threats = GameObject.FindGameObjectsWithTag("Threat");
        if (threats.Length > 0)
            return false;
        else
            return true;
        
    }

    bool FailedObjectiveShips(int minToWin)
    {
        GameObject[] objShips = GameObject.FindGameObjectsWithTag("Objective");
        if (objShips.Length > minToWin)
            return false;
        else
            return true;
    }

    /*

    First priority:
Don't die
Second priority:
Explicit objective
Third objective:
Minimize loss of life






    Defense:
	Asset Guard: Enemies are incoming one wave at a time, and you're holding them off while guarding the objective
		○ Objective is a critical asset that the enemy would like to destroy or control.
			§ Comm Systems
			§ Fold 
		○ Fought from ground Zero
		○ Goal: Prevent Destruction
	
	Repel-All: Enemies are already attacking your objective, and you need to neutralize all enemies in the area, distracting them from their objective to save the asset
	
	Prevention:  A Pinch scenario in which the Enemies are in the process of converting an asset and must be stopped to prevent a tide change.  If you fail, you'll lose the allies and the enemy will be stronger… but in order to succeed you'll sacrifice the lives on the enemy ships, as only destruction will prevent the conversion.
		○ Note: As area score depends largely on life count, a player will need to purposefully fail to complete the objective in order to gain the best solution, though this will involve having a harder end to the level.
	
	Race : you are on the same course as several enemies. 

Offense:
	Asset Grab: Pick off the guards and defenses until you are able to access the asset and reset control
	
	
	
	
	
	
	
	
	Get rid of disengage script
	


    */
}

