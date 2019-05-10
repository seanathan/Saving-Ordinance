using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class defaultToggleValue : MonoBehaviour {
    public ScoreKeeper sc;
    public bool isGrid = false;
    public bool isSmoke = false;

    void Awake()
    {
        sc = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
    }


    void Start()
    {
        // if (ScoreKeeper.scoreKeeperStarted)
        //    AfterScoreStartUp();
        if (isGrid)
        {
            //HUD GRID will stay at default value unless changed.
            int hudPref = PlayerPrefs.GetInt("HUDGrid");

            

            if (hudPref == 1)
                GetComponent<Toggle>().isOn = true;
            else if (hudPref == 2)
                GetComponent<Toggle>().isOn = false;
            else
                GetComponent<Toggle>().isOn = sc.HUDGrid;

        }
        if (isSmoke)
        {
            //Smoke Trails will stay at default value unless changed.
            int smokePref = PlayerPrefs.GetInt("SmokeTrails");

            if (smokePref == 1)
                GetComponent<Toggle>().isOn = true;
            else if (smokePref == 2)
                GetComponent<Toggle>().isOn = false;
            else
                GetComponent<Toggle>().isOn = sc.enableSmokeDrops;
        }

    }
	
}
