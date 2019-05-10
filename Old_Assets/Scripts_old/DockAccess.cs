using UnityEngine;
using System.Collections;

public class DockAccess : MonoBehaviour {
    public bool dockable;
    public GameObject accessGranted;
    public GameObject accessDenied;
    public EnemyShipModular.mobCondition lastState;

    void Start()
    {
        accessGranted.SetActive(false);
        accessDenied.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

        

        if (dockable && accessDenied.activeInHierarchy)
        {
            accessGranted.SetActive(true);
            accessDenied.SetActive(false);

            lastState = GetComponentInParent<EnemyShipModular>().condition;
            GetComponentInParent<EnemyShipModular>().condition = EnemyShipModular.mobCondition.anchored;
            //GetComponentInParent<EnemyShipModular>().active = true;
        }

        if (!dockable && accessGranted.activeInHierarchy)
        {
            accessGranted.SetActive(false);
            accessDenied.SetActive(true);

            GetComponentInParent<EnemyShipModular>().condition = lastState;

            //GetComponentInParent<EnemyShipModular>().active = lastActiveStatus;
        }


        /*

        GetComponentInParent<EnemyShipModular>().takenover = dockable;

        if (dockable)
        {
            GetComponentInParent<EnemyShipModular>().TurnTo(ScoreKeeper.playerAlive);

        }

    */
    }
}
