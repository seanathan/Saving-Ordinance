using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class chassisSwitcher : MonoBehaviour {
    public PlayerControllerAlpha player;
    public Dropdown chassisList;

    void Awake()
    {
        GetComponent<Dropdown>();
    }
    

    // Use this for initialization
    void Start () {
        //identify salvage targets
        if (player == null)
            player = ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>();

        // player.chassis

        
        chassisList.options.Clear();
       // chassisList.options.Add(new Dropdown.OptionData() { text = defaultSelection });
        for (int i = 0; i < player.chassis.Length; i++)
        {
            chassisList.options.Add(new Dropdown.OptionData() { text = player.chassis[i].gameObject.name});

            if (player.chassis[i] == player.activeChassis)
                chassisList.value = i;
        }
        GetComponentInChildren<Text>().text = "Current Chassis: " + player.activeChassis.gameObject.name;

    }

    // Update is called once per frame
    void Update () {
	
	}
}
