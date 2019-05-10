using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobStatus : MonoBehaviour {
    public EnemyShipModular mob;
    public bool debug = true;

    void OnValidate()
    {
        mob = GetComponentInParent<EnemyShipModular>();
        if (mob != null)
            DisplayDebug();

        
    }

    void Start()
    {
        if (mob != null)
            if (mob.GetComponentInChildren<EnemyShipGimbal>() != null)
                transform.GetComponentInParent<Canvas>().transform.SetParent(mob.GetComponentInChildren<EnemyShipGimbal>().transform);
    }

	void Update () {
        if (mob != null)
            DisplayDebug();
        else
        {
            GetComponent<Text>().text = "Enemy Status\nEnemy Status\nEnemyStatus\nEnemyStatus";
        }
	}

    void DisplayDebug()
    {
        GetComponent<Text>().text = mob.ShipStatus(debug);
    }

}
