using UnityEngine;
using System.Collections;

public class ShipGimbal : MonoBehaviour {


	public float bankGimbalForce = -1.0f;

    public float pitchForce = -1.0f;
    public float deltaY = 0.0f;
    public float deltaX = 0.0f;
    private float pitcher;
    private float banker;

    public GameObject shipModel;
    public GameObject cockpit;

    private GameObject gyro;



    public float resetTime = 3.0f;

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
    }

	void FixedUpdate() 
	{
        
        deltaY = 0f;
        deltaX = 0f;


        if (GetComponentInParent<LameShip>() == null)
            return;

        //kill if same object
        if (GetComponent<LameShip>() != null)
            return;

        if (GetComponentInParent<LameShip>().pilot == LameShip.shipController.player)
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

        else if (GetComponentInParent<LameShip>().pilot == LameShip.shipController.mob)
        {
            //angle between gyro and parent
            if (gyro == null)
            {
                if (GetComponentInParent<LameShip>() != null)
                    gyro = GetComponentInParent<LameShip>().getNavGyro();

                else if (GetComponentInParent<EnemyShipModular>() != null)
                    gyro = GetComponentInParent<EnemyShipModular>().navGyro;

            }
            if (gyro == null)
                return;


            deltaY = gyro.transform.localRotation.eulerAngles.y - transform.localRotation.eulerAngles.y;

            if (deltaY > 180)
            {
                deltaY -= 360;
            }

            deltaX = gyro.transform.localRotation.eulerAngles.x - transform.localRotation.eulerAngles.x;

            if (deltaX > 180)
            {
                deltaX -= 360;
            }

            banker = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, deltaY * bankGimbalForce, resetTime * Time.deltaTime);

            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, banker);
  
        }


		banker = Mathf.LerpAngle (transform.localRotation.eulerAngles.z, deltaY * bankGimbalForce, resetTime * Time.deltaTime);
        
		pitcher = Mathf.LerpAngle (transform.localRotation.eulerAngles.x, deltaX * pitchForce, resetTime * Time.deltaTime);
        
		transform.localRotation = Quaternion.Euler (pitcher, 0.0f, banker);
		
	}
    
}
