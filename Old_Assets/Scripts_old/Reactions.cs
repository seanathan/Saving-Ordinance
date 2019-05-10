using UnityEngine;
using System.Collections;

public class Reactions : MonoBehaviour {

    //possible reaction dialogues
    public string[] fiero;

    public string[] relief;

    public string[] remorse;

    public string[] anxiety;

    public string[] panic;

    public string[] loss;

    public string[] frustration;

    //time since last post... limit commentary track!


    public void Fiero(string info = "")
    {
        string message = "";
        int phrase = Mathf.RoundToInt(Random.value * 10 * (fiero.Length -1 )) / 10;
        message += fiero[phrase];
        if (info != "")
            message += "\n" + info;

        NarrationWriter.Ambience(moodEdge.getMoods().fiero);
        NarrationWriter.PopDialogue(message, "Raimi", 3);

    }
    public void Relief(string info = "")
    {
        string message = "";
        int phrase = Mathf.RoundToInt(Random.value * 10 * (relief.Length - 1)) / 10;
        message += relief[phrase];
        if (info != "")
            message += "\n" + info;

        NarrationWriter.Ambience(moodEdge.getMoods().relief);
        NarrationWriter.PopDialogue(message, "Raimi", 3);
    }
    public void Remorse(string info = "")
    {
        string message = "";
        int phrase = Mathf.RoundToInt(Random.value * 10 * (remorse.Length - 1)) / 10;
        message += remorse[phrase];
        if (info != "")
            message += "\n" + info;

        NarrationWriter.Ambience(moodEdge.getMoods().remorse);
        NarrationWriter.PopDialogue(message, "Raimi", 3);
    }
    public void Anxiety(string info = "")
    {
        string message = "";
        int phrase = Mathf.RoundToInt(Random.value * 10 * (anxiety.Length - 1)) / 10;
        message += anxiety[phrase];
        if (info != "")
            message += "\n" + info;

        NarrationWriter.Ambience(moodEdge.getMoods().anxiety);
        NarrationWriter.PopDialogue(message, "Raimi", 3);
    }

    public void Panic(string info = "")
    {
        string message = "";
        int phrase = Mathf.RoundToInt(Random.value * 10 * (panic.Length - 1)) / 10;
        message += panic[phrase];
        if (info != "")
            message += "\n" + info;

        NarrationWriter.Ambience(moodEdge.getMoods().anxiety);
        NarrationWriter.PopDialogue(message, "Raimi", 3);
    }

    public void Loss(string info = "")
    {
        string message = "";
        int phrase = Mathf.RoundToInt(Random.value * 10 * (loss.Length - 1)) / 10;
        message += loss[phrase];
        if (info != "")
            message += "\n" + info;


        NarrationWriter.Ambience(moodEdge.getMoods().remorse);
        NarrationWriter.PopDialogue(message, "Raimi", 3);
    }


    //on score change


    //    Raimi.GetReactions().Fiero(gameObject.name + " has been disabled! Minimal Casualties", gameObject.GetComponent<EnemyShipModular>());


}
