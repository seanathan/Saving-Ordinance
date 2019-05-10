using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueBox : MonoBehaviour {

	public static GameObject tracking;
	public static Text Dialogue;
	public static Slider enemyBar;
	public static Text enemyStatus;
	public Slider eBar;
	public Text eStatus;
	public float resetTimer = 3.0f;
	public float resetcountdown;
	public string dText = "";
	public static string defaultText;
	public string last;
    public bool suppressed = false;

	// Use this for initialization
	void Start () {
		Dialogue = gameObject.GetComponent<Text>();
		enemyBar = eBar;
		enemyStatus = eStatus;
	
		enemyBar.gameObject.SetActive(false);
		enemyStatus.text = " ";
		defaultText = dText;
	}


    // Update is called once per frame
    void Update()
    {
        if (Dialogue.text != last)
        {
            resetcountdown = resetTimer;
        }

        if (resetcountdown < 0.5f)
        {
            Dialogue.text = defaultText;
        }
        if (resetcountdown > 0.0f)
        {
            resetcountdown = resetcountdown - Time.deltaTime;
        }
        last = Dialogue.text;

        
        /*
        if (tracking != null)
        {
            //       enemyBar.gameObject.SetActive(true);
            //       enemyBar.value = (eHP / maxHP);
            //       enemyStatus.text = gameObject.name + "\n" + " Hull : " + eHP + " / " + maxHP;
            //       tracking = gameObject;

            EnemyShipModular mob = tracking.GetComponent<EnemyShipModular>();

            if (mob != null)
                if (mob.mobDialogue != "")
                    Dialogue.text = mob.mobDialogue;
        }*/
    
    }

    public static void Suppress()
    {
        Dialogue.text = "";
    }

    public static void PrintToDBox(string message, GameObject sender)
    {
        Dialogue.text = message;
        GameLog.toLog(message);
        if (DialogueBox.tracking == sender)
        {
            ActivePopUpText.message = message;
        }
    }

    public static void PrintCrash(string message)
    {
        crashText.crashMessage = message;
        Dialogue.text = message;
        
    }
    
}
