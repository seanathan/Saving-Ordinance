using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobileUI : MonoBehaviour {

    public MultiTouchFlightPad MainStick;
    //public VirtualPointerStick FireStick;
    
    public Button FireControl;
    public Button FoldButton;

    public GunCamera MainGunCamera;
    public ForwardFiring FireControlScript;
    public static MobileUI gamecontrols;
    //public PlayerControllerAlpha mainPlayer;
    public JumpFold Fold;

    private void Awake()
    {

        InitUI();
    }

    void InitUI()
    {
        if (gamecontrols == null)
            gamecontrols = this;
        if (MainGunCamera == null)
            MainGunCamera = GameObject.FindGameObjectWithTag("CameraControl").GetComponent<GunCamera>();
        if (FireControlScript == null)
            FireControlScript = GameObject.FindGameObjectWithTag("FireControlScript").GetComponent<ForwardFiring>();
      //  if (mainPlayer == null)
      //      mainPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerAlpha>();
        if (Fold == null)
            Fold = GameObject.FindGameObjectWithTag("FoldDrive").GetComponent<JumpFold>();
    }

    public static MobileUI getControls()
    {

        return gamecontrols;
    }

	
 /*   void Start()
    {
        if (MainGunCamera == null)
            MainGunCamera = GameObject.FindGameObjectWithTag("CameraControl").GetComponent<GunCamera>();

        if (FireControlScript == null)
            FireControlScript = GameObject.FindGameObjectWithTag("FireControlScript").GetComponent<ForwardFiring>();

        if (mainPlayer == null)
            mainPlayer = ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>();

        if (Fold == null)
            Fold = GameObject.FindGameObjectWithTag("FoldDrive").GetComponent<JumpFold>();
    }
    */

    void Update()
    {
        InitUI();

        if (gamecontrols == null)
            gamecontrols = gameObject.GetComponent<MobileUI>(); // pull self
        
        if (FireControl != null)
        {
             FireControl.gameObject.SetActive(PlayerControls.isTiltControlled());
        }
    }

    public void Trigger(bool button = true)
    {
        FireControlScript.Trigger(button);
    }



    public void Throttle(float button)
    {
        PlayerControls.GetActivePlayer().Thruster(button);
        //mainPlayer.Thruster(button);
    }

    //momentary hover
    public void HoverMomentary(bool button)
    {
        PlayerControls.GetActivePlayer().HoverMomentary(button);
    }

    //receive clicks for context
    public void TiltToggle ()
    {
        PlayerControls.GetActivePlayer().TiltToggle();
    }


    public void PauseButtonInput()
    {
        Pause.PauseButton();
    }

    public void FoldingButton ()
    {
        //Fold.Charging();
        Fold.JumpCharge();
    }

    public void FoldDischargeButton ()
    {
        //Fold.Execute();
        Fold.JumpExecute();
    }

    public void LockOn()
    {
        MainGunCamera.trackToggle();
    }
}
