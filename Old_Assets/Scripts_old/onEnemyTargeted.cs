using UnityEngine;
using System.Collections;

public class onEnemyTargeted : MonoBehaviour {

    public Renderer rend;

	// Use this for initialization
	void Start () {
        rend = transform.GetComponent<Renderer>();
        rend.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (DialogueBox.tracking != null)
        {
            GameObject target = DialogueBox.tracking.gameObject;
        
            if ( target.tag == "Threat" || target.tag == "Boss" )
                rend.enabled = true;
            else
                rend.enabled = false;
        }
        else
            rend.enabled = false;
    }
}
