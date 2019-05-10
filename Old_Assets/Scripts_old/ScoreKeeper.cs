using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

[AddComponentMenu("Main GameTracker / Options Manager")]
public class ScoreKeeper : MonoBehaviour {


    //lives remaining
    public int editShipsRemaining = 0;

    public static int ShipsRemaining;

    [Header("Switches")]
    public bool mobileUIOn;


    public static bool mobile;
    public GameObject gridPlanes;
    public bool HUDGrid = true;



    public GameObject radarPing;
    public bool Radar = true;

    public GameObject switchobj1;
    public bool switch1;

    public GameObject switchobj2;
    public bool switch2;

    public bool enableSmokeDrops = false;
    public static bool smokersEnabled = false;

    public static int gameScore;
    public static float playerSpeed;
    public static int soulsMax;
    public static int soulsRrell;
    public static int soulsNative;
    public static int soulsSpared;

    public static GameObject playerAlive;

    //deadmax for each star
    public int deadAllowed3Star = 10;
    public int deadAllowed2Star = 100;
    public int deadAllowed1Star = 1000;
    public static int starScore = 3;

    public float deathFadeOut = 3.0f;
    public bool playerDeathFreakSpin = false;

    public float fixInterval = 0.02f;

    public float AreaScore = 0;
    public static int mapNumber;

    public int totalObjectivesInArea = 10;
    public int objectivesCompleteInArea = 0;
    public static Color threatColor = new Color32(244, 122, 122, 162);
    public static Color allyColor = new Color32(42, 218, 116, 109);

    public static Color hitFlashColor = new Color(1, 0, 0, 1);

    public Color hitFlashColorSet = new Color(1, 0, 0, 1);
    public static float hitFlashRefreshRate;
    public float hitFlashRefreshRateSet = 10.0f;

    public GameObject MobileControls;

    public SelectFromSets statusUI;

    //   public int FrameRateTarget = 60;

    public bool matchFramesToFixed = true;
    public static bool scoreKeeperStarted = false;

    public GameScreen splashes;
    public static bool missionComplete = false;

    public static ScoreKeeper sk;
    public bool dying;
    
    void InitSK()
    {
        GameObject skGO = gameObject;
        if (skGO != null)
            sk = GetComponent<ScoreKeeper>();

        playerAlive = getPlayerAlive();

        dying = false;
        
        if (editShipsRemaining != 0)
        {
            ShipsRemaining = editShipsRemaining;
            editShipsRemaining = 0;
            //PlayerPrefs.SetInt(mapNumber + " ShipsRemaining", ShipsRemaining);
            PlayerPrefs.SetInt("TotalShipsRemaining", ShipsRemaining);
        }
        
        //ShipsRemaining = PlayerPrefs.GetInt(mapNumber + " ShipsRemaining");
        ShipsRemaining = PlayerPrefs.GetInt("TotalShipsRemaining");

    }


    void OnValidate()
    {


        InitSK();

    }

    void Awake() //Start
    {
        /*   //HUD GRID will stay at default value unless changed.
           int hudPref = PlayerPrefs.GetInt("HUDGrid");

           if (hudPref == 1)
               HUDGrid = true;
           else if (hudPref == 2)
               HUDGrid = false;

           //Smoke Trails will stay at default value unless changed.
           int smokePref = PlayerPrefs.GetInt("SmokeTrails");

           if (hudPref == 1)
               enableSmokeDrops = true;
           else if (hudPref == 2)
               enableSmokeDrops = false;

       */

        InitSK();


        if (matchFramesToFixed)
        {
            Application.targetFrameRate = Mathf.RoundToInt(1f / Time.fixedTime);
        }
        else
            Application.targetFrameRate = -1;


        playerSpeed = getPlayerSpeed();
        soulsSpared = 0;
        mapNumber = PlayerPrefs.GetInt("MapLoaded");

        hitFlashColor = hitFlashColorSet;

        gridPlanes = GameObject.FindGameObjectWithTag("GridFloor");
        MobileSet(mobileUIOn);
        Input.simulateMouseWithTouches = false;

        scoreKeeperStarted = true;
    }

    public static GameObject getPlayerAlive()
    {
        if (PlayerControls.GetActivePlayer() != null)
            return PlayerControls.GetActivePlayer().gameObject;

        else
            return null;

    }

    public static float getPlayerSpeed()
    {
        return PlayerControls.getPlayerVelocity();
    }

    // Use this for initialization
    void Update() {
        if (getPlayerAlive() == null)
        {
            Debug.Log("Scorekeeper stopped, no player found");
            return;
        }        
        
        if (storyController.Narrating)
        {
            GetComponent<Canvas>().enabled = false;
            MobileSet(false);
        }
        else
        {
            MobileSet(mobileUIOn);
            GetComponent<Canvas>().enabled = true;
        }
        

        hitFlashColor = hitFlashColorSet;
        hitFlashRefreshRate = hitFlashRefreshRateSet;
        if (AreaScore > PlayerPrefs.GetFloat(mapNumber + " HighScore")) {
            PlayerPrefs.SetFloat(mapNumber + " HighScore", AreaScore);
        } else if (PlayerPrefs.GetFloat(mapNumber + " HighScore") == 0) {
            PlayerPrefs.SetFloat(mapNumber + " HighScore", AreaScore);
        }

        float progress = objectivesCompleteInArea / totalObjectivesInArea;
        PlayerPrefs.SetInt(mapNumber + " TotalObjectives", totalObjectivesInArea);
        PlayerPrefs.SetInt(mapNumber + " ObjectivesComplete", objectivesCompleteInArea);

        if (gridPlanes != null)
            Switch(gridPlanes, HUDGrid);

        if (radarPing != null)
            Switch(radarPing, Radar);

        if (switchobj1 != null)
            Switch(switchobj1, switch1);

        if (switchobj2 != null)
            Switch(switchobj2, switch2);


        soulsRrell = SoulCount("Threat");
        soulsRrell += SoulCount("Boss");
        soulsSpared = SoulCount("NonThreat");
        soulsNative = SoulCount("Ally");
        soulsNative += SoulCount("Objective");

        int dead = ScoreKeeper.soulsMax - ScoreKeeper.soulsRrell - ScoreKeeper.soulsNative;
        if (dead < deadAllowed3Star)
            starScore = 3;
        else if (dead < deadAllowed2Star)
            starScore = 2;
        else if (dead < deadAllowed1Star)
            starScore = 1;                  //minimum star score to complete mission!
        else
            starScore = 0;                  // MISSION INCOMPLETE ON STARSCORE ZERO.


        //deprecated functions...

        playerAlive = getPlayerAlive();
        playerSpeed = getPlayerSpeed();


        //simple endgame text loop
        if (GameObject.FindWithTag("Threat") == null && GameObject.FindWithTag("Boss") == null)
        {
            DialogueBox.Dialogue.text = "All targets neutralized. Score: " + gameScore;
        }

        //DEATH ACTION

        if (PlayerControls.isPlayerDead())
        {
            if (!dying)
            {
                dying = true;

                ShipsRemaining--;

                //PlayerPrefs.SetInt(mapNumber + " ShipsRemaining", ShipsRemaining);
                PlayerPrefs.SetInt("TotalShipsRemaining", ShipsRemaining);
            }

            FadingOut();
        }

        missionComplete = (objectivesCompleteInArea >= totalObjectivesInArea);

        if (objectivesCompleteInArea >= totalObjectivesInArea)
        {
            /*
            splashes.Recap();
            splashes.gameScreenOn = true;
            */

            missionComplete = true;
        }
    }

    bool FadingOut()
    {
        //false if fading or error, true if done

        if (deathFadeOut > 0)
        {

            DialogueBox.Dialogue.text = "PLAYER DEAD";
            GameLog.toLog("PLAYER DEAD");

            deathFadeOut = deathFadeOut - Time.deltaTime;

            /*
            NoiseAndScratches grainOut = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NoiseAndScratches>();

            if (grainOut)
            {

                grainOut.enabled = true;
                grainOut.grainIntensityMax = Mathf.Clamp(5.0f - deathFadeOut, 0.0f, 5.0f);
                grainOut.grainIntensityMin = Mathf.Clamp(4.0f - deathFadeOut, 0.0f, 5.0f);
            }
            */
            return false;
        }
        else if (deathFadeOut > -1)
        {
            //RESET OR GAME OVER
            PlayerLost();

            //drop out of death countdown
            deathFadeOut -= 10f;

            return true;

        }

        return false;
    }

    void PlayerLost()
    {
        PlayerPrefs.Save();

        if (ShipsRemaining > 0)
        {

            //reset to checkpoint
            ResetToCheckpoint();
        }
        else
        {
            LeaveGame();
        }



        
    }

    public static void ResetToCheckpoint()
    {
        //SceneManager.LoadScene(00, LoadSceneMode.Single);

        SceneManager.LoadScene(PlayerPrefs.GetInt("LoadingBarScene"));
    }

    public static void LeaveGame()
    {
        //load main menu
        SceneManager.LoadScene(00, LoadSceneMode.Single);
    }
    
	void Switch (GameObject effect, bool toggle)
	{

        effect.SetActive(toggle);
	}


	int SoulCount(string tagged)
	{
		int souls = 0;

		if (GameObject.FindGameObjectWithTag (tagged) != null) {
			GameObject[] shipsAlive = GameObject.FindGameObjectsWithTag (tagged);


			for (int i = 0; i < shipsAlive.Length; i++) {

				if (shipsAlive [i].GetComponent<EnemyShipModular> () != null)
				{
					souls += shipsAlive [i].GetComponent<EnemyShipModular> ().soulsAlive;
				}
			}
		}
		return souls;

	}

    public void MobileToggle()
    {
        mobileUIOn = !mobileUIOn;
    }

    public void GridSwitch(bool boolish)
    {
        HUDGrid = boolish;
        if (HUDGrid)
            PlayerPrefs.SetInt("HUDGrid", 1);
        if (!HUDGrid)
            PlayerPrefs.SetInt("HUDGrid", 2);


    }

    public void SmokeSwitch(bool boolish)
    {
        enableSmokeDrops = boolish;
        
        if (enableSmokeDrops)
            PlayerPrefs.SetInt("SmokeTrails", 1);
        if (!enableSmokeDrops)
            PlayerPrefs.SetInt("SmokeTrails", 2);
    }

    public void UIDisabler()
    {
        
    }

    void MobileSet(bool setMobile)
    {
        MobileControls.SetActive(setMobile);
        mobile = setMobile;
    }

    public void UiStyle(float slideValue)
    {
        int uiType = Mathf.RoundToInt(slideValue);

        statusUI.SetSelect(uiType);
        
        PlayerPrefs.SetInt("LastUiType", uiType);
    }

}
