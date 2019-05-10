using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour {

    public bool revalidate = false;

    public string[] ammoAliases;

    public string[] hitAliases;


    public GameObject ammoFolder;
    public GameObject effectsFolder;
    public Ammunition[] shots;
    public HitEffect[] splodes;
    public static AmmoManager am;

    private void OnValidate()
    {
        if (revalidate)
            revalidate = false;

        if (getAM() == null)
            return;

        AmmoUpdate();
        EffectUpdate();
    }

    public static AmmoManager getAM()
    {
        if (am == null)
            am = Transform.FindObjectOfType<AmmoManager>();

        if (am == null)
        {
            //if still null, print error
            Debug.Log("Ammo Manager Not Found");
        }

        return am;
    } 
    
    public void AmmoUpdate()
    {
        am = getAM();
        if (am == null)
            return;
        if (am.ammoFolder == null)
            am.ammoFolder = am.gameObject;

        shots = getAM().ammoFolder.GetComponentsInChildren<Ammunition>(true);

        ammoAliases = new string[shots.Length];
        
        if (shots.Length == 0)
        {
            ammoAliases = new string[1];
            ammoAliases[0] = "No Ammo in Ammo Folder";
        }

        for (int i = 0; i < shots.Length; i++)
        {
            ammoAliases[i] = shots[i].getAlias();
        }
    }

    public void EffectUpdate()
    {
        am = getAM();
        if (am == null)
            return;
        if (am.effectsFolder == null)
            am.effectsFolder = am.gameObject;

        splodes = getAM().effectsFolder.GetComponentsInChildren<HitEffect>(true);

        hitAliases = new string[splodes.Length];

        if (shots.Length == 0)
        {
            hitAliases = new string[1];
            hitAliases[0] = "No Effects in Effects Folder";
        }

        for (int i = 0; i < splodes.Length; i++)
        {
            hitAliases[i] = splodes[i].getAlias();
        }
    }

    public static Ammunition getAmmo(string alias)
    {
        Ammunition foundAmmo = null;

        if (getAM() == null)
        {
            Debug.Log("ammo manager not yet accessible");
            return null;
        }

        foreach(Ammunition shot in getAM().shots)
        {
            if (shot.getAlias() == alias)
                foundAmmo = shot;
        }


        if (foundAmmo == null)
        {
            Debug.Log(alias + " does not reference any known ammunition");
        }

        return foundAmmo;
    }

    public static HitEffect getHit(string alias)
    {
        HitEffect foundHit = null;

        foreach (HitEffect splode in getAM().splodes)
        {
            if (splode.getAlias() == alias)
                foundHit = splode;
        }


        if (foundHit == null)
        {
            Debug.Log(alias + " does not reference any known hit effect");
        }

        return foundHit;
    }

    public static bool checkAlias(string alias)
    {
        bool found = false;

        foreach (string al in getAM().ammoAliases)
        {
            if (al == alias)
                found = true;
        }
        foreach (string al in getAM().hitAliases)
        {
            if (al == alias)
                found = true;
        }

        return found;
    }
}