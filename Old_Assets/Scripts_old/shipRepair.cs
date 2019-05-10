using UnityEngine;
using System.Collections;

public class shipRepair : MonoBehaviour {
    
    //when enabled, a player will be restored to full health when docked to this ship
    void Update ()
    {
        if (PlayerControls.getPlayerShip())
        {
            if (PlayerControls.GetActivePlayer().status == PlayerControls.PlayerCondition.Docked)
            {
                if (Vector3.Distance(transform.position, PlayerControls.GetActivePlayer().transform.position) < 500)
                    PlayerControls.getPlayerShip().restoreShip();
            }
        }
        
	}
}
