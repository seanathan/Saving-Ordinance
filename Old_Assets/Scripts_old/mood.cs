using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mood : MonoBehaviour {
    
    [Header("Emotion must be name of gameObject")]
    private string emotion; //NAME OF OBJECT
    public Color tone;
    public string[] phrases;
    private static GameObject emoteFolder;


    public enum emoter
    {
        Raimi,
        Player,
        mob
    }

    public emoter actor = emoter.Raimi;
    
    public mood getEmote(string emote)
    {
        emotion = gameObject.name.ToUpper();
        emote = emote.ToUpper();
        
        if (emote == emotion)
            return this;
        return null;
    }

    private GameObject getFolder()
    {
        if (emoteFolder == null)
            emoteFolder = transform.parent.gameObject;

        return emoteFolder;
    }
    
    public void React(string info = "")
    {
        string message = "";
        int phraseNum = Mathf.RoundToInt(Random.value * 10 * (phrases.Length - 1)) / 10;
        message += phrases[phraseNum];
        if (info != "")
            message += "\n" + info;

        NarrationWriter.Ambience(tone);
        
        if(actor == emoter.Raimi)
            NarrationWriter.PopDialogue(message, "Raimi", 3);
        else
            NarrationWriter.PopDialogue(message, "", 3);



    }

}
