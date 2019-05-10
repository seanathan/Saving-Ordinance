using UnityEngine;
using System.Collections;

public class ReturnHome : MonoBehaviour {
    public EnemyShipModular ship;
    public bool pickup = false;
    // Use this for initialization

    void Awake () {
        ship = GetComponent<EnemyShipModular>();
	}

    void Start()
    {
            ship = GetComponent<EnemyShipModular>();
    }

    void OnValidate()
    {
        if (pickup)
        {
            PickUpPlayer();
        }
        /*
        else
        {
            PickUpDone();
        }
        */
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void PickUpPlayer()
    {
        ship.navigatingTo = ScoreKeeper.playerAlive;
        ship.GetComponentInChildren<DockAccess>().dockable = true;
        ship.throttle = 100;
        ship.condition = EnemyShipModular.mobCondition.active;
    }

    public void PickUpDone()
    {
        if (ship.navigatingTo == ScoreKeeper.playerAlive)
        {
            ship.navigatingTo = null;
            ship.GetComponentInChildren<DockAccess>().dockable = false;

        }
    }
}
