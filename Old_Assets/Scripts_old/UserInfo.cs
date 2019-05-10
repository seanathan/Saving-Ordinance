using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class UserInfo : MonoBehaviour {



    //this holds all of the user settings, preferences, scores, saves, everything

    public string gamerTag = "Player One";
    //user email and login?
    public bool activeUser;

	public bool friendlyFire = true;	//On by default


    public PlayerShip[] ships;

    public PlayerWeapon[] weapons;

    public enum userStatus
    {

		error = -1,

		waiting = 0,

        loading,
        ready,
        writing,
    }

    public userStatus currentStatus;
    public static userStatus status;

    [Header("Preferences")]
    //preferences
    [Range(0, 2)]
    public int quality = 0;

    [Range(0, 2)]
    public int chassis = 0;

    [Range(0, 2)]
    public int UIstyle = 0;

    public float DifficultyLevel = 0;


    public bool smokers = false;
    public bool realisticThrusters = true;
    


    public bool reloadScene = false;

    private ScoreKeeper sk;
    private PlayerControllerAlpha player;
    private static UserInfo local;

    [Multiline]
    public string serialIO;

    private void Awake()
    {
        activeUser = false;
        setStatus(userStatus.waiting);

        if (local != null && local != this)
        {

            activeUser = false;
        }

        setStatus(userStatus.waiting);

        LoadPlayer();


    }

    public static bool checkRealisticThrusterEffects()
    {
        if (getLocal() != null)
            return getLocal().realisticThrusters;
        else
            return true;
    }

    public static float checkStormTrooper()
    {
        if (getLocal() != null)
            return getLocal().DifficultyLevel;
        else
            return 0f;
    }

    public static bool checkSmokerEffects()
    {
        if (getLocal() != null)
            return getLocal().smokers;
        else
            return false;
    }

    public static userStatus getStatus()
    {
        return status;
        
    }

    public static void setStatus()
    {
        //refresh status
        setStatus(status);
    }

    public static void setStatus(userStatus setState)
    {
        status = setState;

        if (getLocal() != null)
            getLocal().currentStatus = status;
    }

    public void LoadPlayer()
    {

        if (status != userStatus.ready) {

            local = this;
            DontDestroyOnLoad(transform.gameObject);
            
            Debug.Log("Local user: " + local.name);


            setStatus(userStatus.ready);
            activeUser = true;
        }
    }
    

    public UserInfo(string name = "Player Default")
    {
        //DO NOT CREATE BLANK IF DEBUG / ERROR
    }
    
    public static void SetQuality(float qualityLevel)
    {
        if (getLocal() == null || status != userStatus.ready)
        {
            //not ready yet.
            return;
        }

        int qual = Mathf.RoundToInt(Mathf.Clamp(qualityLevel, 0, 2));

        

        getLocal().quality = qual;
        //max level is 3

        if (QualitySettings.GetQualityLevel() != qual)
        {
            Debug.Log("Current Quality Setting: " + qual + ", global: " + QualitySettings.GetQualityLevel());

            QualitySettings.SetQualityLevel(qual, true);

            Debug.Log("Changed Quality Setting: " + qual + ", global: " + QualitySettings.GetQualityLevel());

        }


        //  Debug.Log("setting: " + qual + ", global: " + QualitySettings.GetQualityLevel());


    }



    private void OnValidate()
    {
        if (reloadScene)
        {

            ReloadAll();
        }

    }
    
    void pushUpdate()
    {

        EditorSceneManager.preventCrossSceneReferences = false;


    }

    public void setUser(UserInfo loadedUser)
    {
       
        UserInfo old = local;

        if (old != loadedUser)
            Destroy(old);

        local = loadedUser;
        


    }

    public static UserInfo getLocal()
    {
        // do we have local assigned?
        if (local != null)
            return local;

        /*
        //find local
        local = GameObject.FindObjectOfType<UserInfo>();

        //successful?
        if (local != null)
        {

            Debug.Log("Local user: " + local.name);

            return local;
        }*/


        //ELSE RETURN NULL.  

        //PROMPT for new game character?  No, do that at main menu before you get this far.  debug only


        //GameObject defUser = new GameObject("Default User Info");

        //local = defUser.AddComponent<UserInfo>();
        Debug.Log("No user found yet");
        return null;
    }

    public void ReloadAll()
    {

        reloadScene = false;
    }

    public static int getQualityLevel()
    {

        if (status != userStatus.ready || getLocal() == null)
        {

            return 2; //default value for editor.  Set back to zero if it messes up on mobile
        }

        int qual = getLocal().quality;

        SetQuality(qual);

        return qual;
    }
    

    //DO NOT DESTROY.

    //READ IN / READ OUT
    
    // Use this for initialization
    void Start () {
        //wait until scene is loaded to go ready
        LoadPlayer();
    }
	
	// Update is called once per frame
	void Update () {

        setStatus();
        if (activeUser == false)
            gameObject.SetActive(false);

    }
}
