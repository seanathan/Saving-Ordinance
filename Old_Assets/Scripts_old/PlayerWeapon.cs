using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {

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
