using UnityEngine;
using System.Collections;

public class AllyBlasterChange : MonoBehaviour {
    [Header("Deprecated")]

    public GameObject allyShot;
    public GameObject enemyShot;

	// Update is called once per frame
	/*
    
    void Update ()
    {

       
        string alignment = GetComponentInParent<EnemyShipModular>().tag;

        if (alignment == "Threat" || alignment == "Boss")
            GetComponent<EnemyGunTurret>().shot = enemyShot;

        if ( alignment == "Ally" || alignment == "Objective" )
            GetComponent<EnemyGunTurret>().shot = allyShot;
            
    }
    */
}
