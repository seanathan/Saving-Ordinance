using UnityEngine;
using System.Collections;

public class BattleCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
         GetComponent<Canvas>().enabled = !(DialogueBox.tracking == null);
    }
}
