using UnityEngine;
using System.Collections;

public class ScreenPlay : MonoBehaviour {

    public float startDelay = 0f;

    [System.Serializable]
    public struct teleplay
    {

        public string actor;

        [Multiline]
        public string dialogue;
        public float displayTime;
        //ONLY ONE SCREENPLAY AT A TIME!!!!
    }

    public teleplay[] line;

    public int nextLine;
    public string debugMessage = "";
    //ONLY ONE SCREENPLAY AT A TIME!!!!

    public void RunScreenPlay(int next = 0)
    {
        if (nextLine < line.Length)
        {
            NarrationWriter.PopDialogue(line[nextLine].dialogue, line[nextLine].actor, line[nextLine].displayTime);


            debugMessage = "Displaying line " + nextLine + " by " + line[nextLine].actor.ToString();
        }
        
        nextLine++;

        //tell narrator to wait for another line 
        NarrationWriter.followUp = (nextLine < line.Length);
    }
    
    // Update is called once per frame
    void LateUpdate () {

        if (nextLine == 0)
        {
            if (NarrationWriter.waiting)
            {
                debugMessage = "ERROR:  Another narration is already running! Waiting";
                return;
            }
        }

        if (startDelay > 0)
        {
            startDelay -= Time.deltaTime;
            return;
        }

        if (!NarrationWriter.waiting && nextLine < line.Length)
        {
            RunScreenPlay(nextLine);

        }
        
        if (!(nextLine < line.Length))
        {
            debugMessage = "Screenplay finished";
            enabled = false;
        }

    }

    void OnDisable()
    {
        if (nextLine < line.Length)
        {

            NarrationWriter.followUp = false;
        }
    }

}
