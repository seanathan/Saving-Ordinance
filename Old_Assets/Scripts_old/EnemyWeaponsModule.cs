using UnityEngine;
using System.Collections;

public class EnemyWeaponsModule : MonoBehaviour {

    public float weaponsHPMax = 500f;
    public float weaponsHP = 0;

    public bool flash = false;

    public GameObject winDrop;

    private bool weaponStart = true;
    public Renderer FlashBox;

    public GameObject splode;

    private EnemyShipModular myShip;

    public float timeToRepair = 0f;
    private float timeSpentRepairing = 0f;

    void Start()
	{
        myShip = GetComponentInParent<EnemyShipModular>();

        RestoreGun();
	}

    public void BulletHit(BulletScript1 bullet)
    {
        if (bullet.shooter != transform.GetComponentInParent<EnemyShipModular>().gameObject)
        {
            weaponsHP -= bullet.damage;
            flash = true;
        }
    }

    public void RestoreGun()
    {
        weaponsHP = weaponsHPMax;

        if (FlashBox == null && GetComponent<Renderer>() != null)
            GetComponent<Renderer>().enabled = true;
        else if (FlashBox != null)
            FlashBox.enabled = true;

        timeSpentRepairing = 0;
    }

    void HitFlash()
    {
        //FLASH 'FLASHBOX' for 1 frame
        if (FlashBox == null && GetComponent<Renderer>() != null)
            GetComponent<Renderer>().enabled = flash;
        else if (FlashBox != null)
            FlashBox.enabled = flash;
        flash = false;
    }

    void GunDestroyed()
    {
        //WEAPON DESTROYED
        if (gameObject.GetComponentInParent<EnemyShipModular>() != null)
            DialogueBox.PrintToDBox(gameObject.name + " destroyed", gameObject.GetComponentInParent<EnemyShipModular>().gameObject);

        if (winDrop != null)
            Instantiate(winDrop, transform.position, transform.rotation);

        if (splode != null)
            Instantiate(splode, transform.position, transform.rotation);

        //gameObject Disappears
        gameObject.SetActive(false);
    }

    void GunDisabled()
    {
        //WEAPON DESTROYED
        if (gameObject.GetComponentInParent<EnemyShipModular>() != null)
            DialogueBox.PrintToDBox(gameObject.name + " destroyed", gameObject.GetComponentInParent<EnemyShipModular>().gameObject);

        if (winDrop != null)
            Instantiate(winDrop, transform.position, transform.rotation);

        if (splode != null)
            Instantiate(splode, transform.position, transform.rotation);

        //gameObject Disappears
        GetComponentInChildren<EnemyGunTurret>().Rest();

        timeSpentRepairing += Time.deltaTime;

        if (timeSpentRepairing > timeToRepair)
            RestoreGun(); 
    }

    void Update()
	{
        HitFlash();

        weaponsHP = Mathf.Max(weaponsHP, 0f);

        if (weaponsHP <= 0f)
        {
            if (timeToRepair > 0)
                GunDisabled();
            else
                GunDestroyed();
        }



    }
}
