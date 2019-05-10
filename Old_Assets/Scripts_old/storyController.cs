using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class storyController : MonoBehaviour {
	
    //public string AreaDescription;

	public int nextPhase = 0; //phase 0 is reset / uninitialized

    [System.Serializable]
    public struct storyPhases
    {
        public string PhaseDescription;
        public GameObject ActiveOnPhase;
        public GameObject nextPhase;
    };

    public storyPhases[] phases;

//	public GameObject[] phases;
    public bool startCinematicIntroToPhase = false;
	public bool callPhase = false;
    public static bool Narrating = false;
    public bool CinematicOnCall = false;
    public static bool disableCam = false;
    public bool disableCameraWhenNarrating = false;
    //public bool enableNarratorCanvas = false;
    //public Animator cinematics;
    public FadeFromBlack crossfader;
    public bool crossfadeBookend= true;
    public bool callNextPhaseAfterCinematic = false;
    //public Camera cinematicCam;
    public bool isThisNarrating = false;

    public static bool endNarrate = false;
    private float fadeTimer = 0f;
    public static storyController activeStory;

    void Start()
    {
        activeStory = GetComponent<storyController>();
    }

    public void EndCinematic()
    {
        if (!endNarrate)
        {
            fadeTimer = crossfader.fadeClock;
            crossfader.StartFade();
            endNarrate = true;

            foreach (ScreenPlay talker in phases[nextPhase].ActiveOnPhase.GetComponentsInChildren<ScreenPlay>())
                talker.enabled = false;
            NarrationWriter.DismissNow();

            if (callNextPhaseAfterCinematic)
            {
                int newphase = 0;

                if (phases[nextPhase].nextPhase != null)
                    newphase = phaseid(phases[nextPhase].nextPhase);

                if (newphase != 0)
                {
                    nextPhase = newphase;
                }
                else
                    nextPhase++;
            }
        }
    }

    public static void EndCutscene()
    {
        activeStory.EndCinematic();
    }

    public void CallPhaseWithNarrator()
    {
        callPhase = true;
        if (crossfadeBookend)
        {
            fadeTimer = crossfader.fadeClock;
            crossfader.StartFade();
        }
        CinematicOnCall = true;
    }

    public int phaseid(GameObject phaseToSpawn)
    {
        for (int i = 0; i < phases.Length; i++)
            if (phases[i].ActiveOnPhase == phaseToSpawn)
            {
                return i;
            }

        return -1;
    }

	void Update () 
	{
        if (callPhase && fadeTimer <= 0f)
		{
			Phase();
			callPhase = false;
      	}

        if (startCinematicIntroToPhase)
        {
            startCinematicIntroToPhase = false;
            CallPhaseWithNarrator();
        }

        isThisNarrating = Narrating;
        if (fadeTimer > 0f)
            fadeTimer -= Time.deltaTime;


        //Behavior after crossfade
        if (endNarrate && fadeTimer <= 0f)
        {
            //call crossfade, turn off camera.
            //check crossfade duration
            Phase();
            callNextPhaseAfterCinematic = false;
            Narrating = false;
            
            endNarrate = false;
        }

        
        disableCam = disableCameraWhenNarrating;
        
        //call narration mode in editor
            // primaryCam = GameObject.FindGameObjectWithTag("MainCamera");
     
    }

	public void Phase()
	{
		for (int i = 0; i < phases.Length;i++) 
		{
			if (phases[i].ActiveOnPhase != null)
			{
				if (i == nextPhase)
				{
					phases[i].ActiveOnPhase.SetActive(true);
				}
				else
				{
					phases[i].ActiveOnPhase.SetActive(false);
				}
			}
		}

        if (CinematicOnCall)
        {
            Narrating = true;
            CinematicOnCall = false;

        }
    }
}
