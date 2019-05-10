using UnityEngine;
using System.Collections;

[System.Serializable]
public class FiringArc
{
    public float xMin = -180.0f, xMax = 180.0f, yMin = -180.0f, yMax = 180.0f;
}

public class EnemyGunTurret : MonoBehaviour {
        
    public FiringArc arc;            

	public float muzzleV = 500.0f;
	public Transform lagShoot;
	//public GameObject target;
	public Vector3 Vxyz;
	public Transform muzzle;

    public float inaccurate = 0.0f;            //describes variation in accuracy (in degrees).  Set for "normal" difficulty

    public GameObject shot;
	public float fireRate;
	private float nextFire = 0.0f;
    private float firePulse = 0.0f;
	public float fireStagger;

	public GameObject gyro;
    private Quaternion gyroTrack;
    public GameObject rotor;

	private float xmove;
	private float ymove;
	//private float zmove = 0.0f;

	public float swingSpeed = 10.0f;
    public bool isTurret = true;
    private Vector3 gyroAngle;

    public GameObject muzzleFlash;

    public bool Special = false;

    public bool resting = false;

//    public GameObject currentTarget;

    //REQUIRES parent ship to have EnemyShipModular component

    void Start ()
    {
        if (!GetComponentInParent<EnemyShipModular>())
        {
            Debug.Log(name + " does not belong to a mob... disabling");
            enabled = false;
            return;
        }


        if (noAmmo())
        {
            enabled = false;
        }

        if (rotor == null)
            rotor = gameObject;

        if (gyro == null)
        {
            gyro = new GameObject("Gyro Null");
            gyro.transform.position = transform.position;
            gyro.transform.SetParent(transform);

        }

        if (lagShoot == null)
        {
            GameObject lagNull = new GameObject("Lagshot Null");
            lagNull.transform.position = transform.position;
            lagNull.transform.SetParent(transform);

            lagShoot = lagNull.transform;
        }

        if (muzzle == null)
        {
            GameObject muzzleNull = new GameObject("Muzzle Null");
            muzzleNull.transform.position = transform.position;
            muzzleNull.transform.rotation = transform.rotation;
            muzzleNull.transform.SetParent(transform);

            muzzle = muzzleNull.transform;

        }

    }

    public GameObject ControlledFire(GameObject broadTarget)
    {
        if (broadTarget.GetComponent<EnemyShipModular>())
        {
            EnemyShipModular bigship = broadTarget.GetComponent<EnemyShipModular>();

            if (bigship.engines.Length > 0)
            {
                for (int i = 0; i < bigship.engines.Length; i++)
                {
                    if (bigship.engines[i].enginePower > 0)
                        return bigship.engines[i].gameObject;
                }
            }

            if (bigship.weapon.Length > 0)
            {
                GameObject closestGun = bigship.weapon[0].gameObject;
                foreach (EnemyGunTurret gun in bigship.weapon)
                {
                    if (gun != null)
                        if (Vector3.Distance(gun.transform.position, transform.position) < Vector3.Distance(closestGun.transform.position, transform.position))
                            closestGun = gun.gameObject;
                }
                return closestGun;
            }
                
            
        }
        //if nothing else, return broad target

        return broadTarget.gameObject;

        //one mover for the gun, one for the muzzle
    }

    public void Firing(GameObject target, float StormTrooper = 0f)
    {
        if (noAmmo())
        {
            Rest();
            return;
        }

        if (isTurret && target)
        {

            //currentTarget = target;

            // string shipTag = GetComponentInParent<EnemyShipModular>().tag;
            // if (shipTag == "Ally" || shipTag == "Objective")

            target = ControlledFire(target);

            Vxyz = target.GetComponentInParent<Rigidbody>().velocity;

            //perfect accurate
            lagShoot.transform.position = target.transform.position + (Vxyz * (Vector3.Distance(transform.position, target.transform.position)) / muzzleV);

            gyro.transform.LookAt(lagShoot);
            gyroAngle = gyro.transform.localRotation.eulerAngles;

            
            
            // EXPERIMENTAL WITH NO GYROS
            /*
            
            Vector3 AimVector = lagShoot.transform.position - transform.position;
                       
             
            gyroTrack = Quaternion.LookRotation(AimVector, transform.up);  //returns global
          
            gyroAngle = gyroTrack.eulerAngles;
            */            

            //gyroAngle = Vector3.Angle(transform.forward, AimVector.normalized);
            
            
               




            gyroAngle = Angle180.EulerFixer(gyroAngle);

            if (gyroAngle.x < arc.xMax &&
                gyroAngle.x > arc.xMin &&
                gyroAngle.y < arc.yMax &&
                gyroAngle.y > arc.yMin)
            {


                /*
                //NON LERP GYRO WITH NULLS

                   xmove = gyro.transform.rotation.eulerAngles.x;
                   ymove = gyro.transform.rotation.eulerAngles.y;
                   */

                
                //LERP WITH NULL GYROS
                xmove = Mathf.MoveTowardsAngle(rotor.transform.rotation.eulerAngles.x, gyro.transform.rotation.eulerAngles.x, swingSpeed * 2* (1f - StormTrooper) * Time.deltaTime);
                ymove = Mathf.MoveTowardsAngle(rotor.transform.rotation.eulerAngles.y, gyro.transform.rotation.eulerAngles.y, swingSpeed * 2 * (1f - StormTrooper) * Time.deltaTime);
                //zmove = transform.localRotation.eulerAngles.z;

                /*
                //LERP NO GYROS
                xmove = Mathf.MoveTowardsAngle(rotor.transform.rotation.eulerAngles.x, gyroAngle.x, swingSpeed * Time.deltaTime);
                ymove = Mathf.MoveTowardsAngle(rotor.transform.rotation.eulerAngles.y, gyroAngle.y, swingSpeed * Time.deltaTime);
                */




                resting = false;
            }
            else
            {
                Rest();
            }

            //if localRotation > max

            rotor.transform.rotation = Quaternion.Euler(xmove, ymove, transform.rotation.eulerAngles.z);
            //rotor.transform.localRotation = Quaternion.identity.eulerAngles.x;

            //rotor.transform.localRotation = Quaternion.Euler(rotor.transform.localRotation.eulerAngles.x, rotor.transform.localRotation.eulerAngles.y, 0.0f);


            if (!resting)
            {
                //aim muzzle with stormtrooper effect
                //      lagShoot.transform.position = target.transform.position + (Vxyz * (Vector3.Distance(transform.position, target.transform.position)) / muzzleV) + (Random.insideUnitSphere * inaccurate * StormTrooper);


                muzzle.transform.rotation = Quaternion.Euler(xmove + ArcVariance(StormTrooper), ymove + ArcVariance(StormTrooper), transform.rotation.eulerAngles.z);
            }
        }


        if (Time.time > firePulse)
        {
            firePulse = Time.time + fireRate;

            //recalibrate nextfires\
            if (fireStagger < 0.1f)
                FireShot();
            else
                nextFire = firePulse - (fireStagger * fireRate);
        }

        else if (Time.time > nextFire)
        {
            nextFire = nextFire + fireRate;
            if (fireStagger >= 0.1f)
                FireShot();
            else
                nextFire = firePulse - (fireStagger * fireRate);
        }

    }

    float ArcVariance(float stormtrooper = 0f)
    {
        //used to create a variance in the aiming, appearance of innaccuracy
        //adjust for difficulty with "stormtrooper"
        float arcVariance = 1f;
        return (Random.Range(-arcVariance, arcVariance) * inaccurate * stormtrooper);
    }

    public bool noAmmo()
    {        
        if (!shot && !resting)
        {
            
            Debug.Log(name + " has no ammo.. resting");

         //   enabled = false;
            return true;
        }

        return false;
    }

    public void Rest()
	{

        resting = true;

		if (isTurret)
		{


            //gyroTrack = Quaternion.LookRotation(lagShoot.transform.position - transform.position, transform.up);

            //gyro.transform.LookAt(lagShoot);

            //            gyroAngle = gyro.transform.localRotation.eulerAngles;
            //gyroAngle = transform.eulerAngles;

           // gyro = GetComponentInParent<EnemyShipModular>().transform.rotation;

          //  gyroAngle = gyro.transform.localRotation.eulerAngles;

            
                xmove = Mathf.MoveTowardsAngle(rotor.transform.rotation.eulerAngles.x, transform.eulerAngles.x, swingSpeed * Time.deltaTime);
                ymove = Mathf.MoveTowardsAngle(rotor.transform.rotation.eulerAngles.y, transform.eulerAngles.y, swingSpeed * Time.deltaTime);
    
            rotor.transform.rotation = Quaternion.Euler(xmove, ymove, transform.rotation.eulerAngles.z);
		}
	}

    void FireShot()
    {

        GameObject roundFired = (GameObject)Instantiate(shot, muzzle.position, muzzle.rotation);

        roundFired.SendMessage("setShooter", GetComponentInParent<EnemyShipModular>().gameObject, SendMessageOptions.DontRequireReceiver);


        if (roundFired.GetComponent<BulletScript1>())
            roundFired.GetComponent<BulletScript1>().shooterIff = GetComponentInParent<EnemyShipModular>().iff;
        
        if (!isTurret)
        {
           MuzzleBang(muzzle.transform);
        }
        else if (gyroAngle.x < arc.xMax &&
             gyroAngle.x > arc.xMin &&
             gyroAngle.y < arc.yMax &&
             gyroAngle.y > arc.yMin
             && !resting)
        {
           
            //Instantiate(shot, muzzle.transform.position, muzzle.transform.rotation);
            MuzzleBang(muzzle);

            if (roundFired.GetComponent<BulletScript1>() != null)
                roundFired.GetComponent<BulletScript1>().shooterIff = GetComponentInParent<EnemyShipModular>().iff;

        }
    }

    void MuzzleBang(Transform hardpoint)
    {
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, hardpoint.position, hardpoint.rotation, hardpoint);

    }
    
}
