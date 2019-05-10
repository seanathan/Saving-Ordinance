using UnityEngine;
using System.Collections;


public class BulletScript1 : MonoBehaviour {

    public float muzzleVelocity = 50.0f;
	public float muzzleForce = 500.0f;
	public float damage = 5.0f;
	public GameObject hitEffect;
	public float explosiveForce;
	public float explosiveRadius;
	public bool explosion;
	public bool kinetic;
	private bool contact = false;
	public float velocity;
	public float endTime = 0.1f;
    public bool beam = false;
    public GameObject beamTarget;
    public GameObject shooter;
    public EnemyShipModular.mobAffiliation shooterIff;
    public float minDistance = 50;
    private Vector3 initPosition;

    // Use this for initialization

    public GameObject getShooter()
    {
        if (shooter)
            return shooter;

        else
            return ScoreKeeper.playerAlive;
    }

	void Start () {

        
        if (GetComponent<Rigidbody> () != null)
    		GetComponent<Rigidbody> ().AddRelativeForce (Vector3.forward * muzzleForce);


		contact = false;

        transform.SetParent(cleanup.cleaner.transform);
        initPosition = transform.position;

		//get source velocity;
        if (tag == "PlayerBullet" && GetComponent<Rigidbody> () != null) {
			GetComponent<Rigidbody> ().velocity = ScoreKeeper.playerAlive.GetComponent<Rigidbody> ().velocity;
            
		}

	}

    public void setShooter(GameObject newShooter)
    {
        if (!newShooter)
            return;

        //passes IFF to round
        shooter = newShooter;
        
        EnemyShipModular shooterMob = shooter.GetComponentInParent<EnemyShipModular>();
        if (shooterMob != null)
            shooterIff = shooterMob.iff;
        
    }


    void Update()
    {
        //if beam, have the turret control it
        //    if (beam)
        //       transform.parent = shooter.transform;

        if (!shooter)
        {
            shooterIff = EnemyShipModular.mobAffiliation.Ally;
        }        
        
    }

    void OnTriggerEnter(Collider other)
    {
        //        if (minDistance < Vector3.Distance(transform.position, initPosition))
        //        {
        //            Hit(other);
        //        }

        EnemyShipModular ship = other.GetComponentInParent<EnemyShipModular>();
        if (ship != null)
            if (ship.gameObject == shooter)
                return;

    //    if (minDistance < Vector3.Distance(transform.position, initPosition))
    //        return;

        Hit(other);
    }

	void LateUpdate()
	{
		if (contact)
			Destroy(gameObject, endTime);
    }

    void Hit(Collider other)
    {        
        other.SendMessageUpwards("BulletHit", gameObject.GetComponent<BulletScript1>(), SendMessageOptions.DontRequireReceiver);
        
        Instantiate(hitEffect, transform.position, transform.rotation);

        contact = true;

        //kinetic force
        if (explosion == true)
        {
            if (other.GetComponentInParent<Rigidbody>())
            {
                other.GetComponentInParent<Rigidbody>().AddExplosionForce(explosiveForce, transform.position, explosiveRadius);
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
