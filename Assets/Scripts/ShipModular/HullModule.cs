using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullModule : ShipModule{
	//This is the Main Hull of the Ship.

	//There MAY BE MULTIPLE HULLS, but only one should be the gimbal

//	public bool isPrimaryHull{
//		get { return ( GetComponentInParent<HullModule>() == null); }
//	}
	
	//throw this into a module??
	/*
	public float hullHP = 100f;
	public float currentHP = 100f;    //current HP
	public float bracerHP = 0;
	public float bracerRegenRate;
	*/

	//player only:
	//public GameObject cockpit;

	/*
    public bool cockpitMode(bool onCockpit)
    {
        if (cockpit == null || shipModel == null)
        {
            Debug.Log("Can not access cockpit mode. please designate cockpit");
            return false;
        }
        else
        {
            cockpit.SetActive(onCockpit);
            shipModel.SetActive(!onCockpit);
        }

        return onCockpit;
    }*/

	/*
	public void gimbalAction(){


		//FOr PLAYER USE
		/*
		if (GetComponentInParent<SpaceShip>().pilot == SpaceShip.shipController.player)
        {
            if (PlayerControls.GetActivePlayer() != null)
            {
                if (PlayerControls.GetActivePlayer().status == PlayerControls.PlayerCondition.Active)
                {
                    deltaY = PlayerControls.GetActivePlayer().mH;

                    deltaX = PlayerControls.GetActivePlayer().mV;
                }
            }
        }

		//angle between gyro and parent
		
		_banker = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, deltaY * bankGimbalForce, resetTime * Time.deltaTime);
		
		_pitcher = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, deltaX * pitchForce, resetTime * Time.deltaTime);
		
		transform.localRotation = Quaternion.Euler(_pitcher, 0.0f, _banker);

	}*/

	
}