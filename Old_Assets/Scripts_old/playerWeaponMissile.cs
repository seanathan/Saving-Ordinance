using UnityEngine;
using System.Collections;

public class playerWeaponMissile : MonoBehaviour {

    public bool turret = false;


    public float shotCost = 5.0f;


    public Transform mainFire1;
    public Transform mainFire2;
    public Transform altFire1;
    public Transform altFire2;

    public float fireRate;
    public float nextFire;
    public float nextFire2;
    public float nextFire3;
    public float nextFire4;
    public GameObject shot;

    public bool turretAim = false;




    public void fireWeapon(GameObject aimSpot)
    {
       if (mainFire1)
            mainFire1.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        if (mainFire2)
            mainFire2.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);



        //alternating fire


        if (Time.time > nextFire && PlayerControllerAlpha.charge > shotCost && mainFire1)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, mainFire1.position, mainFire1.rotation);
            ShotCost(shotCost);

            //recalibrate nextfires
            nextFire2 = nextFire - (0.75f * fireRate);
            

        }
        
        else if (Time.time > nextFire2 && PlayerControllerAlpha.charge > shotCost && mainFire2)
        {
            nextFire2 = nextFire2 + fireRate;
            Instantiate(shot, mainFire2.position, mainFire2.rotation);

            ShotCost(shotCost);
        }
        
    }


    public void ShotCost(float cost)
    {
        PlayerControllerAlpha.charge = PlayerControllerAlpha.charge - cost;
    }

}
