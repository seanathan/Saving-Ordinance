using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDTracker : MonoBehaviour {
    
	private GameObject[] allies;
	private GameObject[] objectives;
	private GameObject[] Dock;
	private GameObject[] threats;
	private GameObject[] junks;
	public GameObject[] salvages;
	private GameObject[] bosses;
	private GameObject[] nonthreats;

    [System.Serializable]
    public struct HUDColoring
    {
        public string tagType;
        public Color tagColor;
        //shape?
    }
    
    public HUDColoring[] HUDColors;
    
    public GameObject boxHUD;
    private GameObject[] allreticules;
     
	private GameObject activeHUD;
	public float rfScale = 10.0f;

	public float sensorRange = 2000;
	public float MAXsensorRange = 8000.0f;
	public int activeTargetIndex = 0;

	public GameObject[] allSelectable;

	public GameObject raimiTarget;
	public GameObject activeTarget;

	public GameObject hudMaster;

    public GameObject lagShoot;
    public GameObject targetingPick;

    public float muzzleV = 800.0f;  //typical bullet velocity
    public bool aimAssist = true;
    public ActivePopUpText popup;

    [System.Serializable]
    public struct HUDObject
    {
        public GameObject bogey;
        public GameObject ret;

    };

    public HUDObject[] bogeys = new HUDObject[100];
    public int bogeyCount = 0;
    

    // Use this for initialization
    void Start ()
    {
		UpdateTagged ();
        targetingPick.SetActive(false);
        
    }


    // Update is called once per frame
    void Update () {

		UpdateTagged ();
        
        //get all objects in range

        //if object is untagged, do not include


        activeTarget = DialogueBox.tracking;


    }

	public void RunWithCamera()
	{
        //new
        AssignHUDtoBogeys(objectives);

        AssignHUDtoBogeys(Dock);

        AssignHUDtoBogeys(allies);

        AssignHUDtoBogeys(threats);

        AssignHUDtoBogeys(junks);

        AssignHUDtoBogeys(salvages);

        AssignHUDtoBogeys(bosses);

        AssignHUDtoBogeys(nonthreats);


        //old
        //run this as part of the camera late update;
        //check for range

        if (aimAssist && activeTarget != null && activeTarget.tag != "Ally" && activeTarget.tag != "Objective" && activeTarget.GetComponent<Rigidbody>() != null)
		{
			targetingPick.SetActive(true);
			lagShoot.transform.position = activeTarget.transform.position + (activeTarget.GetComponent<Rigidbody>().velocity * (Vector3.Distance(transform.position, activeTarget.transform.position)) / muzzleV);

			targetingPick.transform.LookAt(lagShoot.transform);


		}
        else
		{
			targetingPick.SetActive(false);
		}

        if (popup != null)
        {
            popup.TrackOnHUD();
        }
	}

    HUDObject GetHUDRet(GameObject trackedObject)
    {
        for (int i = 0; i < bogeyCount; i++)
        {
            //find reticule
            if (bogeys[i].bogey == trackedObject)
                return bogeys[i];

            
        }


        //else make one

        bogeys[bogeyCount].bogey = trackedObject;

        bogeys[bogeyCount].ret = (GameObject)Instantiate(boxHUD, hudMaster.transform);
        bogeys[bogeyCount].ret.name = trackedObject.GetInstanceID().ToString();
        bogeyCount++;

        return bogeys[bogeyCount - 1];
    }

    void AssignHUDtoBogeys(GameObject[] things)
    {
        foreach (GameObject bog in things)
        {
            BogeyOnHUD(bog);
        }
    }

    void ColorOnTag(HUDObject bogeyHUD)
    {
        for (int i = 0; i < HUDColors.Length; i++)
        {
            if (HUDColors[i].tagType == bogeyHUD.bogey.tag)
                bogeyHUD.ret.GetComponentInChildren<Image>().color = HUDColors[i].tagColor;
        }
        
    }

    void ColorWithRange(HUDObject bogeyHUD, bool inRange = true)
    {
        Image retColor = bogeyHUD.ret.GetComponentInChildren<Image>();
        
        for (int i = 0; i < HUDColors.Length; i++)
        {
            if (HUDColors[i].tagType == bogeyHUD.bogey.tag)
            {
                if (inRange)
                    retColor.color = Color.Lerp(retColor.color, HUDColors[i].tagColor, Time.deltaTime);
                else
                    retColor.color = Color.Lerp(retColor.color, 
                        new Color(HUDColors[i].tagColor.r, HUDColors[i].tagColor.g, HUDColors[i].tagColor.b, 0f), 
                        Time.deltaTime);
            }
        }
    }

    void BogeyOnHUD(GameObject bogey)
    {

        //Assign a HUD reticule for this thing
        //assign appropriate sprite
        //assign appropriate color
        //display faded or off if in distance
        //display UI info if in range
        //display ACTIVE UI info if ACTIVE target

        //ERROR CATCH
        if (bogey == null)
            return;

        HUDObject HUDItem = GetHUDRet(bogey);

        //don't display if maximum HUD elements exceeded
        if (bogeyCount >= bogeys.Length)
            return;

        
        //zero position
        HUDItem.ret.transform.localPosition = Vector3.zero;

        //line up with object in view
        HUDItem.ret.transform.LookAt(bogey.transform.position, hudMaster.transform.up);
        
        //distance to target
        float rangeTarget = Vector3.Distance(bogey.transform.position, ScoreKeeper.playerAlive.transform.position);
        
        //find HUD renderer
        Canvas HUD = HUDItem.ret.GetComponentInChildren<Canvas>();

        HUDItem.ret.transform.GetComponentInChildren<ThreatHP>().uiCanvas.enabled = true;
        ThreatHP statusBar = HUDItem.ret.transform.GetComponentInChildren<ThreatHP>();

        string enemyStatus = "";
        string enemyIFF = "";
        string mobHull = "";

        if (rangeTarget < sensorRange && bogey.GetComponent<EnemyShipModular>())
        {

            mobHull = bogey.GetComponent<EnemyShipModular>().eHP + " / " + bogey.GetComponent<EnemyShipModular>().maxHP;



            //display HPBar on hostile targets
            statusBar.hpbar.value = (bogey.GetComponent<EnemyShipModular>().eHP / bogey.GetComponent<EnemyShipModular>().maxHP);
            
            if (bogey.tag == "Threat")
            {
                 }

            else if (bogey.tag == "NonThreat")
            {
                enemyStatus = "DISABLED:";
                Color disabledColor = new Vector4(48.0f / 255.0f, 172.0f / 255.0f, 234.0f / 255.0f, 255.0f / 255.0f);
                statusBar.hpcolor.color = disabledColor;
            }

            else if (bogey.tag == "Objective")
            {
                enemyStatus = "OBJECTIVE:";
                Color objColor = new Vector4(0.0f / 255.0f, 255.0f / 255.0f, 33.0f / 255.0f, 202.0f / 255.0f);
                statusBar.hpcolor.color = objColor;
            }

            else if (bogey.tag == "Boss")
            {
                Color bossColor = new Vector4(255.0f / 255.0f, 0.0f / 255.0f, 237.0f / 255.0f, 255.0f / 255.0f);
                enemyStatus = "BOSS:";
                statusBar.hpcolor.color = bossColor;
            }

            else if (bogey.tag == "Salvage")
            {
                enemyStatus = "DESTROYED / SALVAGE";
                mobHull = "";
                statusBar.hpbar.gameObject.SetActive(false);
            }


            //statusUpdate
            //statusBar.hptxt.text = bogey.name + ": " + (Mathf.Round(rangeTarget) * rfScale) + "m" + "\n" + enemyStatus;
            enemyIFF = "<b>" + bogey.name + ":</b> " + (Mathf.Round(rangeTarget) * rfScale) + "m";

            //					if (thing.GetComponent<EnemyShipModular>().battleMode == false || thing.GetComponent<EnemyShipModular>().eHP <1.0f)
            //					{
            //						statusBar.hptxt.text = thing.GetComponent<EnemyShipModular>().tag;
            //					}
        }
        else if (rangeTarget < sensorRange && bogey.tag == "Objective")
        {   //statusUpdate
            //statusBar.hptxt.text = bogey.name + ": " + (Mathf.Round(rangeTarget) * rfScale) + "m" + "\n" + "<b>OBJECTIVE</b>";
            enemyIFF = "<b>" + bogey.name + ":</b> " + (Mathf.Round(rangeTarget) * rfScale) + "m" + "\n" + "<b>OBJECTIVE</b>";

            statusBar.hpbar.gameObject.SetActive(false);
        }

        else
        {
            HUDItem.ret.transform.GetComponentInChildren<ThreatHP>().uiCanvas.enabled = false;
        }


        //ColorOnTag(HUDItem);

        ColorWithRange(HUDItem, (rangeTarget < MAXsensorRange));


        //enable renderer if in range
        //HUD.enabled = (rangeTarget < MAXsensorRange);

        //check if selectable


        //enemyIFF = bogey.name + ": " + (Mathf.Round(rangeTarget) * rfScale) + "m";

        statusBar.statusText(enemyStatus, enemyIFF, mobHull);

    }
    
	void UpdateTagged()
	{
		//update tagged object lists
		
		objectives = GameObject.FindGameObjectsWithTag ("Objective");
		Dock = GameObject.FindGameObjectsWithTag ("Dock");
		allies = GameObject.FindGameObjectsWithTag ("Ally");
		threats = GameObject.FindGameObjectsWithTag ("Threat");
		junks = GameObject.FindGameObjectsWithTag ("Junk");
		salvages = GameObject.FindGameObjectsWithTag ("Salvage");
		bosses = GameObject.FindGameObjectsWithTag ("Boss");
		nonthreats = GameObject.FindGameObjectsWithTag ("NonThreat");
	}
    
}


