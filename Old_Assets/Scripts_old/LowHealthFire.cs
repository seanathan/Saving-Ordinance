using UnityEngine;
using System.Collections;

public class LowHealthFire : MonoBehaviour {
    public GameObject fires;
    public float HPpoint = 0.2f;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GetComponentInParent<LameShip>())
            fires.SetActive(GetComponentInParent<LameShip>().getHealth().ratio < HPpoint);

        else
            fires.SetActive(false);
        
	}
}
