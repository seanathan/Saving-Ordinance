using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameLog : MonoBehaviour {
    
    
    //CATCHES ALL TEXT AND LOG INFO FROM GAME


    public static string newLogText = "";
    public bool editorpush = false;
    public static Text logText;
    public int maxLines;
    public string totalLog = "";


    void Awake()
    {
        logText = GetComponent<Text>();
    }

	// Update is called once per frame
	void Update () {
        char[] lines = logText.text.ToCharArray();
        int lineCount = 0;
        foreach (char ch in lines)
            if (ch == '\n')
                lineCount++;

        if (lineCount > maxLines)
            logText.text.TrimStart('\n');


        totalLog = newLogText;


    }

    public static void toLog(string message)
    {

        GameLog.logText.text += "\n" + message;
        newLogText += "\n" + message;
    }
}
