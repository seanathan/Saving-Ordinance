using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FiringArc2
{
    public float xMin = -180.0f, xMax = 180.0f, yMin = -180.0f, yMax = 180.0f;
}

public class ShipWeapon : MonoBehaviour {


    public bool turret = false;

    //public GameObject muzzleFlash;
    public bool turretAim = false;

    public float shotCost = 5.0f;

    public int maxPoints = 4;

    private int nextWeapon;
    private Transform[] hardpoints;

    public float fireRate;
    private float nextFire;

    public string ammo;
    public Ammunition shot;


    public FiringArc arc;

    public float muzzleV = 500.0f;
    public Transform lagShoot;
    //public GameObject target;
    public Vector3 Vxyz;
    public Transform muzzle;

    public float inaccurate = 0.0f;            //describes variation in accuracy (in degrees).  Set for "normal" difficulty
    
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


    public string reticule;


    private void Start()
    {
        getPoints();
    }

    private void OnValidate()
    {
        shot = AmmoManager.getAmmo(ammo);
    }

    public Transform nextGun()
    {
        //if null just return the whole transform of the ship

        Transform gun = null;

        if (hardpoints.Length > 0)
        {
            if (nextWeapon >= hardpoints.Length || nextWeapon >= maxPoints)
                nextWeapon = 0;

            gun = hardpoints[nextWeapon];

            nextWeapon++;
        }

        if (gun == null)
            gun = transform;

        return gun;
    }

    public Transform[] getPoints()
    {
        LameShip ship = PlayerControls.getPlayerShip();

        Transform[] points = null;

        if (ship != null)
        {
            return ship.hardPoints;
        }

        //return gun controller if no points assigned

        points = new Transform[1];
        points[0] = this.transform;

        return points;
    }


    public void fireWeapon(GameObject aimSpot)
    {
        if (shot == null)
        {
            Debug.Log(name + " is shooting blanks");

            return;
        }

        //will always return at least the center of this object
        hardpoints = getPoints();

        foreach (Transform gunPoint in hardpoints)
        {
            if (turret && turretAim)
            {
                gunPoint.LookAt(aimSpot.transform.position);

            }
            else
            {
                gunPoint.LookAt(transform.forward);

            }
        }



        //alternating fire

        if (Time.time > nextFire && PlayerControllerAlpha.charge > shotCost)
        {
            Transform gun = nextGun();

            if (gun == null)
                return;

            nextFire = nextFire + fireRate;
            //Instantiate(shot, altFire1.position, altFire1.rotation);
            Ammunition.Fire(shot, gun);

            ShotCost(shotCost);

        }

    }


    public void ShotCost(float cost)
    {
        PlayerControllerAlpha.charge -= cost;
    }

}
