using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class inertialSafeties : MonoBehaviour {

    public float dragOutput;

    private PlayerControls player;
    private Text counterText;
    private Button safetyButton;
    public string preMessage;
    public string outMessage;
    public float reducedDrag;
    //public float safetyDrag;
    public float changerate;
    private float changerTimer = 0f;

    public bool dragfree = true;
    
    public float velThreshold = 300f;
    public Color off = new Color32(255, 255, 255, 178);
    public Color on = new Color32(0, 255, 0, 178);
    
    

    void Start()
    {
        dragfree = false;
    //    player = PlayerControllerAlpha.GetActivePlayer();
        safetyButton = GetComponent<Button>();
        counterText = GetComponentInChildren<Text>();
    }
    
	// Update is called once per frame
	void Update ()
    {
     //   if (player.GetComponent<PlayerControllerAlpha>() == null)
        //    PlayerControllerAlpha.GetActivePlayer();

        player = PlayerControls.GetActivePlayer();


        if (Input.GetButtonDown("Back Button"))
            SafetyToggle();


        if (dragfree && ThrottleSafe())
        {
            SafetyOff();
            counterText.text = string.Format(preMessage, "OFF");
        }
        else
        {
            SafetyOn();
            counterText.text = string.Format(preMessage, "ON");
        }
    }


    bool ThrottleSafe()
    {
        //flag true if good to roam free, false if unsafe

        if (ScoreKeeper.playerSpeed > velThreshold)
            return false;

        if (player.th == 0)
            return true;

        return true;
    }

    void changePacer(bool up = true)
    {
        int neg = -1;
        if (up) neg = 1;


        changerTimer += neg * changerate * Time.deltaTime;

    }

    void SafetyOff()
    {
        //changePacer();
        safetyButton.image.color = Color.Lerp(safetyButton.image.color, on, changerTimer);

        dragOutput = Mathf.MoveTowards(PlayerControls.getPlayerShip().getRigidBody().drag, reducedDrag, changerate * Time.deltaTime);

        PlayerControls.getPlayerShip().DynamicDrag(dragOutput);
    }

    void SafetyOn()
    {
        safetyButton.image.color = Color.Lerp(safetyButton.image.color, off, changerate * Time.deltaTime);
        dragOutput = Mathf.MoveTowards(PlayerControls.getPlayerShip().getRigidBody().drag, PlayerControls.getPlayerShip().normalDrag, changerate * Time.deltaTime);

        PlayerControls.getPlayerShip().DynamicDrag(dragOutput);
        //dragfree = false;
    }

    public void SafetyToggle(bool force = false, bool forcedValue = true)
    {
        if (force)
            dragfree = forcedValue;
        else
            dragfree = !dragfree;
    }


    public void SafetyToggleButton()
    {
        SafetyToggle();
    }
}
