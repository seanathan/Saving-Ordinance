using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mimicDialogue : MonoBehaviour {
    public bool Dialogue = false;
    public bool HPStatus = false;
    public bool otherText = false;
    public string HPFormat = "HULL: {0} / {1}";
    public Text otherTextField;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (Dialogue)
            GetComponent<Text>().text = DialogueBox.Dialogue.text;

        if (otherText)
            GetComponent<Text>().text = otherTextField.text;

        if (HPStatus)
        {
            if (PlayerControls.getPlayerShip() != null)
                GetComponent<Text>().text = string.Format(HPFormat, PlayerControls.getPlayerShip().getHealth().currentHP, PlayerControls.getPlayerShip().getHealth().maxHP);
        }
    
    }

}
