using UnityEngine;
using System.Collections;

public class resetEnemies : MonoBehaviour {
    public bool reset = false;
	// Use this for initialization

    void Start()
    {
        if (reset)
            ResetAll();
    }

    void Update()
    {
        if (reset)
            ResetAll();
    }

	void ResetAll () {
        EnemyShipModular[] NPCships = gameObject.GetComponentsInChildren<EnemyShipModular>();
        for (int i = 0; i < NPCships.Length; i++)
        {
            NPCships[i].reset = true;
            NPCships[i].RestoreShip();
        }
        reset = false;
	}
	
}
