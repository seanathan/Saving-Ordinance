using UnityEngine;
using System.Collections;


public class EngineGimbal : MonoBehaviour {
	//needs player horizontal
	//needs player vertical
    
	public float rotorSpeed;
	public float tilt;
	public float bank;
	public float pitch;
	public bool Left;


    [Header("Experimental")]
    public bool realistic = false;

    private float nextPullTime;
    public float timeToPullAngle = 0.1f;
    public Vector3 deltaRotation;
    public Vector3 lastRotation;
    public float adjustmentScalar = 1f;
    public float xRot;

    private float lastX = 0f;
    private float lastZ = 0f;

    public float rotRate = 10f;
    
    public bool isOnPlayerShip()
    {
        LameShip ship = GetComponentInParent<LameShip>();
        PlayerControls player = PlayerControls.GetActivePlayer();
        if (player != null)
        {
            if (player.activeVessel == ship)
                return true;

            if (ship.pilot == LameShip.shipController.player)
                return true;
        }

        return false;
    }

    void initTrackers()
    {

        tilt = 0.0f;
        bank = 0.0f;
        pitch = 0.0f;

       // playerAccel = GetComponentInChildren<ThrusterPowerMirror>();

      //  player = PlayerControls.GetActivePlayer();

        if (!isOnPlayerShip())
            return;

        if (PlayerControls.GetActivePlayer().activeVessel == null)
            return;

        Transform gimbal = PlayerControls.GetActivePlayer().activeVessel.transform;

        /*
        if (playerAccellReferenceNull == null)
        {
            playerAccellReferenceNull = new GameObject("Player Accelleration Reference");
            playerAccellReferenceNull.transform.SetParent(gimbal);
            playerAccellReferenceNull.transform.localPosition = Vector3.zero;
        }

        if (playerAccellPointerNull == null)
        {
            playerAccellPointerNull = new GameObject("Player Accelleration Pointer");
            playerAccellPointerNull.transform.SetParent(gimbal);
            playerAccellPointerNull.transform.localPosition = Vector3.zero;
        }
        */
    }

    float DegreeClamp(float degreeRaw)
    {
        if (Mathf.Abs(degreeRaw) > 180)
            degreeRaw -= 360 * Mathf.Sign(degreeRaw);

        return degreeRaw;
    }

    void RocketSpin()
    {
        Vector3 torque = Vector3.zero;
        
    }


    // Update is called once per frame
    void FixedUpdate () {


        //   bank = player.mH;
        //   pitch = -player.mV;

        if (!isOnPlayerShip())
            return;

        bank = PlayerControls.GetActivePlayer().mH;
        pitch = -PlayerControls.GetActivePlayer().mV;

        //if still banked and controls are released, run opposite
            
        float leftFlip = 1;
        if (Left)
            leftFlip = -1;
        

        tilt = (leftFlip * bank) + pitch;
        tilt = Mathf.Clamp(tilt, -90f, 90f);

        transform.localRotation = Quaternion.Euler(tilt * rotorSpeed, 0.0f, 0.0f);
        
        
	}
}
