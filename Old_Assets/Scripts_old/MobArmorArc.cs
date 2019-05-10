using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobArmorArc : MonoBehaviour {

    public bool shields;
    public bool HP;


    // Update is called once per frame
    void Update () {
        float eHP = GetComponentInParent<EnemyShipModular>().eHP;
        float eHPmax = GetComponentInParent<EnemyShipModular>().maxHP;
        //get the current Shield or HP, display
//        if (shields)
//            GetComponent<Image>().fillAmount = (eHP/ eHPmax);
//        if (HP)
            GetComponent<Image>().fillAmount = (eHP/ eHPmax);
    }
}
