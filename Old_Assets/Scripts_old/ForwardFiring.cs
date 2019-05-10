using UnityEngine;
using System.Collections;

public class ForwardFiring : MonoBehaviour {
    
    public GameObject firingZero;
    public GunCamera mainCameraShell;
    public PlayerControllerAlpha player;


	private bool triggerValue = false;
    
    public GameObject activeRet;
    public SelectFromSets reticules;



    public string weaponCallByAlias;
    public GameObject activeWeapon;
    public SelectFromSets playerWeapons;
    
    public int currentGun = 0;

    public bool turretAim = false;
    

    void Start ()
    {
        if (player == null)
            player = ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>();

        //  availableGuns = player.activeChassis.transform
    //    availableGuns = GameObject.FindGameObjectsWithTag("PlayerGun"); 

        mainCameraShell = GameObject.FindGameObjectWithTag("CameraControl").GetComponent<GunCamera>();
        turretAim = false;
        firingZero = GameObject.FindGameObjectWithTag("fireZero");

        WeaponSwitch(currentGun);
    }

    private void OnValidate()
    {
        if (weaponCallByAlias.Length > 0)
            WeaponSwitch(weaponCallByAlias);
    }

    void Update()
    {
        //debug weapon list refresh
        //   WeaponSwitch(currentGun);

        turretAim = mainCameraShell.freeLook;

        Trigger(Input.GetButton("Fire2"));
        
    }
    void LateUpdate()
    {
        if (turretAim)
        {
            transform.LookAt(firingZero.transform.position);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }


    public void Trigger(bool firing = true)
    {
        if (firing)
            getActiveWeapon().fireWeapon(firingZero);
    }

    public void RetSwitch()
    {
        PlayerWeapon weapon = getActiveWeapon();

        activeWeapon.GetComponent<PlayerWeapon>();
        reticules.SetSelect(weapon.reticule);
        
    }

    public void WeaponSwitch(int gunIndex)
    {
        playerWeapons.SetSelect(gunIndex);
        
        RetSwitch();
    }
   

    public void WeaponSwitch(string alias)
    {
        playerWeapons.SetSelect(alias);

        RetSwitch();
    }

    public PlayerWeapon getActiveWeapon()
    {
        if (playerWeapons.getActiveSet() == null)
            return null;
        
        activeWeapon = playerWeapons.getActiveSet().gameObject;

        PlayerWeapon weapon = activeWeapon.GetComponent<PlayerWeapon>();

        return weapon;
    }

    public void nextWeapon()
    {
        playerWeapons.nextSet();

        PlayerWeapon weapon = getActiveWeapon();

        activeWeapon.GetComponent<PlayerWeapon>();
        reticules.SetSelect(weapon.reticule);
    }

}
    