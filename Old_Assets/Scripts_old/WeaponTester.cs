using UnityEngine;
using System.Collections;

public class WeaponTester : MonoBehaviour {

    public bool fireTest = false;
    public GameObject target;
    void Update()
    {
        if (fireTest)
            GetComponent<EnemyGunTurret>().Firing(target, 1);
    }
}
