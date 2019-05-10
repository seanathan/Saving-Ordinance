using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NarrationWriter : MonoBehaviour {

    public float displayTime = 4f;  //default display time
    public static float defaultTime;
    public static float displayCountDown = 0f;
    //private string lastMessage = "";
    public static string outMessage = "";
    public static string actor = "";
    public Text ActorNameDisplay;
    public Text MessageOutputDisplay;

    [TextArea(5, 10)]
    public string TESTMESSAGE = "";
    public string TESTACTOR = "Test";
    public static bool waiting = false;
    public float stallTime = 0.2f;
    private float stall = 0f;
    private Color initColor;
    public Image panelBackdrop;
    public static bool followUp = false;    //followup lets the program know that it should pull the next Dialogue instead of dismissing
    public static NarrationWriter thisWriter;
    private float firstSecond = 0f;
    

    void Awake()
    {
        thisWriter = GetComponent<NarrationWriter>();
    }

    void Start()
    {
        initColor = panelBackdrop.color;

        thisWriter = GetComponent<NarrationWriter>();
    }

    public static void Next()
    {
        thisWriter.Dismiss();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("A Button"))
            Next();

        defaultTime = displayTime;

        //dialogue displayer
        MessageOutputDisplay.text = outMessage;

        if (firstSecond > 0f)
            firstSecond -= Time.deltaTime;

        // display countdown minder
        if (waiting && (displayCountDown <= 0f || outMessage == ""))
        {
            Dismiss();
        }
        else
        {
            DialogueBox.Suppress();
            displayCountDown -= Time.deltaTime;
        }
        
        //Actor text minder
        if (outMessage != "" && actor != "")
        {
            ActorNameDisplay.text = actor + ":";
        }
        else
        {
            ActorNameDisplay.text = "";
        }

        //canvas minder
        
        //stall after waiting
        if (stall > 0f && !waiting && !followUp)
        {
            //fade the background
            Color stallColor = initColor;
            stallColor.a = Mathf.Lerp (0f, stallColor.a, Mathf.Pow(stall / stallTime, .2f));

            //stallColor.a = Mathf.MoveTowards(stallColor.a, 0f, Time.deltaTime * stallTime);
            panelBackdrop.color = stallColor;

            stall -= Time.deltaTime;
        }
        else
            panelBackdrop.color = initColor;
            
        //GetComponent<Canvas>().enabled = ((stall > 0f || waiting) && !Pause.paused);
        

        GetComponent<Canvas>().enabled = ((stall > 0f || waiting || followUp ) && !Pause.paused);
     

        //display tester
        if (TESTMESSAGE != "")
        {
            PopDialogue(TESTMESSAGE, TESTACTOR);
            TESTMESSAGE = "";
        }
        
    }

    public static void Ambience(Color moodLight)
    {
        moodEdge.MoodSwitch(moodLight);
    }

    public static void DismissNow()
    {
        thisWriter.Dismiss();
        waiting = false;
    }

    public void Dismiss()
    {
        if (outMessage == "")
            return; // already dismissed?
        

        //don't dismiss if less than 1 second from last dismiss
        if (firstSecond > 0f)
            return;

        if (!followUp)
            outMessage = "";
        waiting = false;
        
        
        stall = stallTime;
        Ambience(moodEdge.getMoods().neutral);

        firstSecond = 1f;
    }
    

    public static void PopDialogue(string message = "", string speaking = "", float duration = -1f)
    {
        //wait for button?
        //pause?

        if (duration > 10f)
        {
            //press anywhere in box to dismiss

            //display "tap to dismiss" note

            //clear text when button pressed
        }

        if (duration <= 0)
            duration = defaultTime;  //use default display countdown time if negative value passed

        displayCountDown = duration;

        outMessage = message;
        actor = speaking;

        waiting = true;
    }
}
