using UnityEngine;
using System.Collections;

public class HUDSpinWithCharge : MonoBehaviour {
    public bool Cap = false;
    public bool fold = false;
    public float spinSpeed = 1f;
	
	// Update is called once per frame
	void Update () {
        if (PlayerControls.GetActivePlayer() == null)
            return;

        if (Cap)
            transform.Rotate(Vector3.forward, spinSpeed * PlayerControls.GetActivePlayer().charge / PlayerControls.GetActivePlayer().chargeMax *Time.deltaTime);

        if (fold)
            transform.Rotate(Vector3.forward, spinSpeed * (0.1f + JumpFold.foldPower) * Time.deltaTime);

    }
}
