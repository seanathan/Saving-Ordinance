using UnityEngine;
using System.Collections;

public class phoenixGimbal : MonoBehaviour {

    public float fullSize;
    public float fullSpeed;
    //	public float fullBright;
    //UNUSED    private Color normalColor;

    [Header("Experimental")]

    public bool realistic = false;
    public float deltaVelocity = 0f;
    private Vector3 lastVelocity = Vector3.zero;

    public Vector3 accelDirection = Vector3.zero;

    public float realThrustScalar = 1f;
    public float timeToPullSpeed = .1f;
    private float nextPullTime = 0f;
    private GameObject nullDirectionReference;
    private GameObject ship;
    public bool reversable = false;
    public bool reactive = false;

    private GameObject shipAccellReferenceNull;
    private GameObject shipAccellPointerNull;
    public float lowEndTolerance = 0.1f; // in terms of delta v
    public float lowEndTorqueTolerance = 1f; //in terms of scaled deltaRot

    public float deltaRot = 0f;
    public float torqueScalar = 0f;

    // Use this for initialization
    void Start()
    {

        ship = transform.root.gameObject; //get the absolute highest parent

        if (nullDirectionReference == null)
        {
            nullDirectionReference = new GameObject("Null Direction Reference");

            EngineGimbal rocketVector = GetComponentInChildren<EngineGimbal>();
            if (rocketVector != null)
                nullDirectionReference.transform.SetParent(rocketVector.transform);
            else
                nullDirectionReference.transform.SetParent(transform);
            nullDirectionReference.transform.localPosition = Vector3.forward;

        }

        if (shipAccellReferenceNull == null)
        {
            shipAccellReferenceNull = new GameObject("Ship Accelleration Reference");
            shipAccellReferenceNull.transform.SetParent(ship.transform);
            shipAccellReferenceNull.transform.localPosition = Vector3.one;
        }
        if (shipAccellPointerNull == null)
        {
            shipAccellPointerNull = new GameObject("Ship Accelleration Pointer");
            shipAccellPointerNull.transform.SetParent(ship.transform);
            shipAccellPointerNull.transform.localPosition = Vector3.zero;
        }

        //	smoker = GetComponentInChildren<DropSmoke> ();
        //UNUSED        normalColor = GetComponentInChildren<ParticleSystem>().startColor;

        ThrustOff();
    }


    void FixedUpdate()
    {

        if (ship == null)
        {
            return;
        }





        float thThrust = rocketThrust();

        float thTorque = 0f;

        if (torqueScalar > 0f)
        {
            deltaRot = 0f;

            if (ship.GetComponentInChildren<EnemyShipGimbal>())
                deltaRot = ship.GetComponentInChildren<EnemyShipGimbal>().deltaY;

            if (ship.GetComponentInChildren<ShipGimbal>())
                deltaRot = ship.GetComponentInChildren<ShipGimbal>().deltaY;

            deltaRot = deltaRot / torqueScalar; //factor out angles
            //torque of thruster
            //distance from center axis of ship, multiplied by deltaRot

            float armLength = 0f;
            //0 armLength should act normally

            // armLength = thEffect.transform.localPosition.x;
            //forward will always be the direction the thrust is acting
            //distance from point to a line:

            armLength = -Vector3.Dot(transform.position - ship.transform.position, ship.transform.right);


            thTorque = Mathf.Sign(armLength) * deltaRot;

            if (Mathf.Abs(deltaRot) > lowEndTorqueTolerance)
                thThrust += thTorque;

            //reclamp
            thThrust = Mathf.Clamp(thThrust, -1, 1);

        }


        if (thThrust >= 0.0f)
        {

           // thEffect.startSize = fullSize * thThrust;


            if (reactive)
            {
            //    thEffect.startSize = fullSize * (thThrust - (ship.GetComponent<PlayerControllerAlpha>().thrust * 0.1f));

            }
           // thEffect.startSpeed = fullSpeed;
            //GetComponentInChildren<Light>().intensity = fullBright * rocketThrust();


        }

        else if (thThrust < 0.0f && reversable)
        {

           // thEffect.startSize = -fullSize * thThrust;
          //  thEffect.startSpeed = -fullSpeed;
            //GetComponentInChildren<Light>().intensity = fullBright * Mathf.Abs(rocketThrust());

        }

        else
            ThrustOff();


    }
      
    public Vector3 AccelVector()
    {
        Vector3 rotationEuler = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(Vector3.zero);

        rotationEuler.x = Vector3.Dot(accelDirection, transform.right);
        //  rotationEuler.y = Vector3.Dot(accelDirection, transform.up);
        rotationEuler.z = Vector3.Dot(accelDirection, transform.forward);
        shipAccellReferenceNull.transform.localPosition = accelDirection.normalized;

        shipAccellPointerNull.transform.LookAt(shipAccellReferenceNull.transform);



        return rotationEuler;

        //transform.localRotation = shipAccellPointerNull.transform.localRotation;



        //return rotation;
    }

    float rocketThrust()
    {
        float thrust = 0f;

        //if (ship.GetComponentInChildren<EnemyShipModular>())
          //  if (ship.GetComponentInChildren<EnemyShipModular>().enginesDisabledFlag || ship.GetComponentInParent<EnemyShipModular>().dying)
                return 0f;

    //    if (ship.GetComponentInParent<PlayerControllerAlpha>())
            if (ship.GetComponentInParent<PlayerControllerAlpha>().crashCountdown > .1)
                return 0f;

        //normal control-based reflection
        if (!realistic && GetComponentInParent<PlayerControllerAlpha>() != null)
            thrust = ship.GetComponent<PlayerControllerAlpha>().thrust * 0.1f; // limit thrust value between 0 and 1

        if (realistic)
        {
            //use ship velocity as guide, versus change in velocity
            if (Time.time > nextPullTime)
            {
                //calculate rocket thrust
                Vector3 deltaVel3 = ship.GetComponent<Rigidbody>().velocity - lastVelocity;
                //pointer in direction of acceleration
                accelDirection = deltaVel3.normalized;

                deltaVelocity = Vector3.Dot(
                    deltaVel3,
                    (nullDirectionReference.transform.position - transform.position));

                lastVelocity = ship.GetComponent<Rigidbody>().velocity;

                nextPullTime = Time.time + timeToPullSpeed;
            }

            thrust = deltaVelocity;

            if (Mathf.Abs(thrust) < lowEndTolerance)  //kill engines if below tolerance
                return 0;

            thrust = thrust * realThrustScalar;

        }

        thrust = Mathf.Clamp(thrust, -1f, 1f);
        return thrust;

    }




    void ThrustOff()
    {
        //reset angles
    }
}

