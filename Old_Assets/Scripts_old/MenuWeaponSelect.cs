using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuWeaponSelect : MonoBehaviour {
    public ForwardFiring gunControl;
    public Dropdown weaponList;
    void Awake()
    {
        GetComponent<Dropdown>();
    }

    void Start()
    {

        if (gunControl == null)
            gunControl = ScoreKeeper.playerAlive.GetComponent<ForwardFiring>();


        gunControl.WeaponSwitch(0);
        RefreshList();
        weaponList.value = 0;


        // RefreshList();
    }

    void Update()
    {

        RefreshList();

    }


    void RefreshList()
    {
     //   gunControl.WeaponSwitch(0);
        //identify salvage targets


        // player.chassis


        weaponList.options.Clear();
        

        // chassisList.options.Add(new Dropdown.OptionData() { text = defaultSelection });
        for (int i = 0; i < gunControl.playerWeapons.Sets.Length; i++)
        {
            weaponList.options.Add(new Dropdown.OptionData() { text = gunControl.playerWeapons.Sets[i].getAlias() });
            
        }
        GetComponentInChildren<Text>().text = "Current Weapon: " + gunControl.getActiveWeapon().gameObject.name;

    }

}
