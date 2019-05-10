using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SalvageOptions : MonoBehaviour {

	public Dropdown availableTargets;
	public string tagScan;
	public string defaultSelection;
	public float rfScale = 10.0f;
	public GameObject player;
    public static GameObject enemyTarget;



    void Update() {
        //identify salvage targets
        player = ScoreKeeper.playerAlive;
        GameObject[] scannedTags = GameObject.FindGameObjectsWithTag(tagScan);
        availableTargets.options.Clear();
        availableTargets.options.Add(new Dropdown.OptionData() { text = defaultSelection });
        foreach (GameObject bogey in scannedTags)
        {
            availableTargets.options.Add(new Dropdown.OptionData() { text = bogey.name + ": " + (Mathf.Round(Vector3.Distance(player.transform.position, bogey.transform.position)) * rfScale) + "m" });
        }

        if (availableTargets.value != 0)
        {  enemyTarget = scannedTags[availableTargets.value - 1]; }

        else if (availableTargets.value == 0)
        {
            enemyTarget = null;
        }
    }


//	public void FindSalvage ()
//	{
//
//		//identify salvage
//		GameObject[] scannedTags = GameObject.FindGameObjectsWithTag (tagScan);
//
//		//for each salvage, add the gameObject to the pulldown
//
//
//		//for each salvage, add the gameObject to the pulldown
//		availableTargets.options.Add(new Dropdown.OptionData() {text = "blah"});
//
//
//
//
////		foreach (GameObject bogey in scannedTags) 
////		{
////
////			//add to the options
////			//availableTargets.options[1] = "Blah"
////
////		}
//
//	}
}
