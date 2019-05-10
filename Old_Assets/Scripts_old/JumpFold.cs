using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpFold : MonoBehaviour {

    public enum foldState
    {
        waiting = 0,
        inactive,

        discharging,

        charging = 5,
        peaked,
        engaged,

    }

    public foldState state;

	public float JumpDistance = 0.0f;
	public float advRate = 10.0f;
	public int advPower = 2;
	//public float minJump = 50.0f;
	public float jumpDelay = 0.0f;
	//public Vector3 jumpTo = Vector3.zero;
	private GameObject player;
	public GameObject charge;
	public GameObject disCharge;
    public float maxScale = 1f;

    public bool dropper = false;
 // public bool FoldDriveEngaged = false;
	public float maxJump = 3000.0f;
	public float rfScale = 10.0f;
//  private bool maxReached = false;
    

    public GameObject foldIcon;

    public float foldCost = 1.0f;
    public static float foldPower = 0f;


    [Header("Fixed Jumps")]

    public bool fixedJumpEnabled = false;
    public float fixedJumpDistance = 2000f;
    public float fixedJumpChargeTime = 1f;

//    public static bool buttonPressed = false;
//    public static bool buttonReleased = false;
 //   public bool disengage = false;

 //   public bool foldObstructed = false;
    public JumpClearance clearAhead;
    public bool doubleTap = false;

	void Start () 
	{

        player = ScoreKeeper.playerAlive;
        
        Reset();

    }

    public void JumpCharge()
    {
        state = foldState.charging;
        
        jumpDelay = 1f;

    }

    public void JumpExecute()
    {

        state = foldState.discharging;
    }

    void stateUpdate()
    {
        /*  public enum foldState
      {
          waiting,
          inactive,
          charging,
          peaked,
          engaged,
          discharging,

      }*/
      

        if (state == foldState.waiting)
        {
            state = foldState.inactive;
        }

        if (state == foldState.charging)
        {
            Charging();
        }

        if (state == foldState.discharging)
        {
            Execute();
            Reset();
        }


    }



    /// <summary>
    /// jump needs to test if it's in a trigger.  if yes, move further by increments of 100 
    /// 
    /// drop to world space instead of parented space??
    /// </summary>

    bool obstructed()
    {
        if (clearAhead)
            if (clearAhead.jumpObstructed)
                return true;
        
        return false;
    }


    public void FixedJump(float FixedJumpDistance = 1000f)
    {
        state = foldState.peaked;

        JumpDistance = FixedJumpDistance;
        jumpDelay = fixedJumpChargeTime;
        fixedJumpEnabled = true;

  //      FoldDriveEngaged = true;
    }

    void Charging()
    {
        if (JumpDistance < maxJump && PlayerControllerAlpha.charge > 0.0f)
        {
            JumpDistance += advRate * Time.deltaTime;  //  ((chargeTime * advRate) * (chargeTime * advRate)) + minJump;

            PlayerControllerAlpha.charge -= (foldCost * Time.deltaTime);
        }
        else
        {
            state = foldState.peaked;
        }
        

        clearAhead.gameObject.SetActive(true);
        
        //instantiate jump effect charge
        if (dropper)
            Instantiate(charge, player.transform.position, player.transform.rotation);
        else
            charge.SetActive(true);


        if (jumpDelay > 0)
            jumpDelay -= Time.deltaTime;
        else
            Execute();

        DialogueBox.Dialogue.text = "Fold Jump Distance: " + Mathf.RoundToInt(JumpDistance * rfScale) + "m";

        foldIcon.transform.localPosition = Vector3.forward * JumpDistance;
        clearAhead.transform.position = foldIcon.transform.position;
    }

    void Execute()
    {

        state = foldState.peaked;

        //RaycastHit foldClearance;
        //Ray FoldAhead = new Ray(foldIcon.transform.position, player.transform.forward);

        //if (Physics.Raycast(FoldAhead, out foldClearance, 1f))
        //if (player.GetComponent<Rigidbody>().SweepTest(player.transform.forward, out foldClearance, JumpDistance))

        if (obstructed())
        {
            //Debug.Log(foldClearance.collider.name);
            DialogueBox.Dialogue.text = "Fold Jump Obstructed";
            GameLog.toLog("Fold Jump Obstructed");
            jumpDelay = fixedJumpChargeTime;
            return;
        }

        state = foldState.discharging;


        //disengage = true;
        player.transform.position = foldIcon.transform.position;
        
        DialogueBox.Dialogue.text = "Fold Jump Successful";
        GameLog.toLog("Fold Jump Successful");
      
        //instantiate jump pop
        Instantiate(disCharge, player.transform.position, player.transform.rotation);
        
    //  Reset();
    }

    public void Reset()
    {
        state = foldState.waiting;



  //      disengage = false;
    //    maxReached = false;
        if (!dropper)
            charge.SetActive(false);
        
        JumpDistance = 0f;
        jumpDelay = 0f;
  //      maxReached = false;
    //    FoldDriveEngaged = false;

   //     buttonPressed = false;
     //   buttonReleased = false;

        clearAhead.gameObject.SetActive(false);
        //foldIcon.transform.localPosition = Vector3.zero;
    }

    void FixedUpdate ()
	{
        stateUpdate();

        //     if (disengage && FoldDriveEngaged)
        //        Reset();


        if (Input.GetButtonDown("Fold"))
            JumpCharge();


        if (state == foldState.charging || state == foldState.peaked)
        {
            if (Input.GetButtonUp("Fold"))
                JumpExecute();



        }
        
        //icon is active if not charging/discharging
        foldIcon.SetActive((int)state >4);

        foldPower = JumpDistance / maxJump;
    }
    /*
    void LateUpdate()
    {
        foldIcon.transform.localPosition = Vector3.forward * JumpDistance;
        
        clearAhead.transform.position = foldIcon.transform.position;
    }*/
}
