using UnityEngine;
using System.Collections;

public class MissilePursuitScript : MonoBehaviour {


    public float muzzleVelocity = 50.0f;
    public float muzzleForce = 500.0f;
    public float manueverForce = 10f;
    public float damage = 5.0f;
    public GameObject hitEffect;
    public float explosiveForce;
    public float explosiveRadius;
    public bool explosion;
    public bool kinetic;

    private bool contact = false;
    public float velocity;
    public float endTime = 0.1f;
    public float seekTime = 15f;
    public float noSeekTime = 5f;
    public bool beam = false;
    public GameObject beamTarget;

    public GameObject gyro;

    public float minDistance = 50;
    private Vector3 initPosition;
    public GameObject target;

    private float mark1 = 0.0f;
    private float heading1 = 0.0f;
    private float bank1 = 0.0f;

    public float xTurn = 1000f;
    public float yTurn = 1000f;
    public float zTurn = 1000f;

    public float rollRate = 0.5f;


    

    // Use this for initialization
    void Start()
    {
        //		Vector3 shooting = new Vector3 (0.0f, 0.0f, muzzleForce);
        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * muzzleForce);


        contact = false;

        transform.SetParent(cleanup.cleaner.transform);
        initPosition = transform.position;

        //get source velocity;
        if (tag == "PlayerBullet" && GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().velocity = ScoreKeeper.playerAlive.GetComponent<Rigidbody>().velocity;

        }

        target = DialogueBox.tracking;

        if (target != null)
            Invoke("AbortMissile", seekTime);
        else
            Invoke("AbortMissile", noSeekTime);
    }
    
    void AbortMissile()
    {
        Collider abort = null;
        Hit(abort);
    }

    void FixedUpdate()
    {
        
        if (target != null)
        {

           gyro.transform.LookAt(target.transform);

            TurnTo(gyro);

            if (GetComponent<Rigidbody>() != null)
                GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * manueverForce);

        }
    }

    
    void OnTriggerEnter(Collider other)
    {

        if (beam)
            beamTarget = other.gameObject;
        if (minDistance < Mathf.Abs(Vector3.Distance(transform.position, initPosition)))
        {
            Hit(other);


        }
    }

    void LateUpdate()
    {
        if (GetComponent<Rigidbody>() != null)
            velocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        if (contact)
        {
            Destroy(gameObject, endTime);
        }


    }

    void Hit(Collider other)
    {
        GameObject HIT = (GameObject)Instantiate(hitEffect, transform.position, transform.rotation);
        contact = true;

        if (other != null)
        {

            //hit splash
           

            //cause damage
            other.SendMessageUpwards("BulletImpact", damage, SendMessageOptions.DontRequireReceiver);



            //explode, match explosion velocity
            if (other.GetComponentInParent<Rigidbody>())
            {
                Vector3 vel = other.GetComponentInParent<Rigidbody>().GetPointVelocity(transform.position);
                HIT.GetComponent<Rigidbody>().velocity = vel;
                //OBSOLETE                EllipsoidParticleEmitter[] sploderFix = HIT.GetComponentsInChildren<EllipsoidParticleEmitter>();
                //OBSOLETE                sploderFix[0].worldVelocity = vel;
                //OBSOLETE                sploderFix[1].worldVelocity = vel;
            }



            //kinetic force
            if (explosion == true)
            {
                if (other.GetComponentInParent<Rigidbody>())
                {
                    other.GetComponentInParent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveRadius);
                    //other.GetComponent<Rigidbody> ().AddForce(explosiveForce * GetComponent<Rigidbody>().velocity);
                }
            }

            //alt kinetic force
            if (kinetic == true)
            {
                if (other.GetComponentInParent<Rigidbody>())
                {
                    other.GetComponentInParent<Rigidbody>().AddForceAtPosition(explosiveForce * GetComponentInParent<Rigidbody>().velocity, transform.position);
                }
            }
        }
    }



    void TurnTo(GameObject wayPoint)
    {
        //new heading
        float markNav = wayPoint.transform.rotation.eulerAngles.x;
        float headingNav = wayPoint.transform.rotation.eulerAngles.y;
        float bankNav = wayPoint.transform.localRotation.eulerAngles.z;

        if (mark1 == 270.0f)
        {
            mark1 = 280.0f;
        }

        //active heading
        heading1 = Mathf.LerpAngle(heading1, headingNav, yTurn * Time.deltaTime);
        mark1 = Mathf.LerpAngle(mark1, markNav, xTurn * Time.deltaTime);



        bank1 = Mathf.LerpAngle(bank1, bankNav, zTurn * Time.deltaTime);


        Quaternion newcourse = Quaternion.Euler(new Vector3(mark1, heading1, bank1 * rollRate));

        //			transform.localRotation = player.transform.rotation;


        //transform.localRotation = newcourse;

        transform.rotation = newcourse;
        
    }
}
