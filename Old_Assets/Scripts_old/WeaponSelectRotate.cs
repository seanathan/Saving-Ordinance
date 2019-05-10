using UnityEngine;
using System.Collections;

public class WeaponSelectRotate : MonoBehaviour {
    // Use this for initialization

    private ForwardFiring fc;

    void Start()
    {
        fc = MobileUI.gamecontrols.FireControlScript;
    }

    public void WeaponCycle()
    {
      //  int newGun = fc.currentGun + 1;
     //   if (newGun >= fc.availableGuns.Length)
     //       newGun = 0;
     //   fc.WeaponSwitch(newGun);
        //print new weapon to floatText
   //     DialogueBox.Dialogue.text = fc.availableGuns[newGun].name + " Selected";
    }
}
