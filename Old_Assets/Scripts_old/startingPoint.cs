using UnityEngine;
using System.Collections;

public class startingPoint : MonoBehaviour {
	public GameObject areaObject;
    public bool isPlayer = false;

    [Header ("Mob Controls")]
    private EnemyShipModular mobShip;
    public bool ally = false;
    public bool threat = false;
    public GameObject firstNav;

    public EnemyShipModular.mobCondition Condition;
    public EnemyShipModular.mobAffiliation Affiliation;
    public EnemyShipModular.combatMode CombatDisposition;

    public bool assault = false;
    public GameObject Objective;

    public bool resting = false;

    public float difficulty = 1.0f;     //1 is "normal" difficulty
	// Use this for initialization
	void Start () {
        if (isPlayer)
            areaObject = ScoreKeeper.playerAlive;

        Spawn();
	}

    public void Spawn()
    {
        if (areaObject != null)
        {
            areaObject.transform.position = transform.position;
            areaObject.transform.rotation = transform.rotation;

            mobShip = areaObject.GetComponent<EnemyShipModular>();
            if (mobShip != null)
            {
                mobShip.stormTrooper = difficulty;

                mobShip.condition = Condition;
                mobShip.SetAlignment(Affiliation);
                mobShip.combat = CombatDisposition;

                mobShip.nextDestination = firstNav;

                mobShip.assaultMode = assault;

                if (assault)
                    mobShip.assaultTarget = Objective;


                if (resting)
                    mobShip.condition = EnemyShipModular.mobCondition.anchored;
                else
                    mobShip.condition = EnemyShipModular.mobCondition.active;
            }
        }
        

    }
}
