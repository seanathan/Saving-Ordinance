using UnityEngine;
using System.Collections;

public class ThrusterPowerMirror : MonoBehaviour {
//	public float fullBright;
	public DropSmoke smoker;


    [Range(0f, 3f)]
    public float tuningScale = 1f;

    [System.Serializable]
    public struct Thruster
    {
        public ParticleSystem effect;
        public float fullSize;
        public float fullSpeed;
        public float thThrust;
    }
    public Thruster[] allThrusters;

    // private ParticleSystem[] thrustEffects;

    public float clampValue = 1f;

    public float realThrustScalar = 1f; //order of magnitude for velocity and accel

    private LameShip ship;
    public bool reversable = false;

    public float reactionSpeed = 1f;
    
    public float lowEndTolerance = 0.1f; // in terms of delta v
    public float lowEndTorqueTolerance = 1f; //in terms of scaled deltaRot
    
    public float deltaRot = 0f;
    public float torqueScalar = 0f;


    public bool idling = false;

    public LameShip getShip()
    {
        if (ship == null)
            ship = GetComponentInParent<LameShip>();
        return ship;
    }
    
    // Use this for initialization
    void Start () {

        ParticleSystem[] thrusters = GetComponentsInChildren<ParticleSystem>();

        allThrusters = new Thruster[thrusters.Length];

        for (int i = 0; i < allThrusters.Length; i++)
        {
            allThrusters[i].effect = thrusters[i];
            allThrusters[i].fullSize = thrusters[i].main.startSize.constant;
            allThrusters[i].fullSpeed = thrusters[i].main.startSpeed.constant;
            allThrusters[i].thThrust = 0f;
        }

        //       fullBright = GetComponentInChildren<Light>().intensity ;

        ThrustOff();
    }
    
    void FixedUpdate () 
	{

        if (getShip() == null)
            return;


        deltaRot = 0f;

        if (getShip().getShipGimbal())
        {


            deltaRot = ship.getShipGimbal().deltaY;

            if (torqueScalar != 0f)
                deltaRot = deltaRot / torqueScalar; //factor out angles
                                                    //torque of thruster
                                                    //distance from center axis of ship, multiplied by deltaRot

        }


        for (int i = 0; i < allThrusters.Length; i++)
        {
            if (allThrusters[i].effect != null)
            {
                allThrusters[i].thThrust = rocketThrust(allThrusters[i].effect.gameObject);
                var thmain = allThrusters[i].effect.main;
                float thTorque = 0f;

                if (torqueScalar != 0f)
                {
                    

                    float armLength = 0f;
                    //0 armLength should act normally

                    // armLength = thEffect.transform.localPosition.x;
                    //forward will always be the direction the thrust is acting
                    //distance from point to a line:

                    armLength = -Vector3.Dot(allThrusters[i].effect.transform.position - ship.transform.position, ship.transform.right);


                    thTorque = Mathf.Sign(armLength) * deltaRot;

                    if (Mathf.Abs(deltaRot) > lowEndTorqueTolerance)
                        allThrusters[i].thThrust += thTorque;

                    //reclamp


                }

                allThrusters[i].thThrust *= tuningScale;

                allThrusters[i].thThrust = thrusterClamp( allThrusters[i].thThrust);


             
                    thmain.startSize = Mathf.Abs(allThrusters[i].thThrust) * allThrusters[i].fullSize;
                    thmain.startSpeed = allThrusters[i].fullSpeed * allThrusters[i].thThrust;
            

               /* else if (allThrusters[i].thThrust < 0.0f && reversable)
                {
                    thmain.startSize = Mathf.Abs(allThrusters[i].thThrust) * allThrusters[i].fullSize;
                    thmain.startSpeed = allThrusters[i].fullSpeed * allThrusters[i].thThrust;
                }*/

               // else
             //       ThrustOff();
            }
        }
        
        if (smoker)
        {

            smoker.enabled = (ScoreKeeper.smokersEnabled && (Mathf.Abs(getShip().getVelocity()) > 0.5f));
        }

    }


    
    float rocketThrust(GameObject thruster)
    {
        float thrust = 0f;

        if (getShip() == null)
            return 0f;
        
        thrust = Vector3.Dot(
                    getShip().getAccelForce(),
                    (thruster.transform.forward));
        
        if (Mathf.Abs(thrust) < lowEndTolerance)  //kill engines if below tolerance
              return 0f;

        thrust = thrust / realThrustScalar;

        thrust = thrusterClamp(thrust);


        //ENGINES ARE OPPOSITE OF FORCE EXERTED
        return -thrust;
        
    }


    float thrusterClamp(float th)
    {
        if (clampValue > 0f)
            return Mathf.Clamp(th, -clampValue, clampValue);
        else
            return th;

    }

    void ThrustOff()
    {

        foreach (Thruster th in allThrusters)
        {
            if (th.effect != null)
            {
                float thThrust = rocketThrust(th.effect.gameObject);
                var thmain = th.effect.main;

                thmain.startSize = 0f;
                thmain.startSpeed = 0f;
                }
        }
    }
}
