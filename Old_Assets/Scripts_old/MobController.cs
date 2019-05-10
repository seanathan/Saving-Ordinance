using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour {

    public LameShip activeVessel;


	// Update is called once per frame
	void Update () {
        if (activeVessel == null)
            GetComponent<LameShip>();
        

	}
}
