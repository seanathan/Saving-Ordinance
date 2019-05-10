using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeFromBlack : MonoBehaviour {
    public Image blackness;
    //public GameObject MobileControls;

    public bool crossFade = false;
    public float fadeClock = 3.0f;
    private float fadeCountdown;
    public bool callFade = true; // default setting
    public bool fading = false;

	// Use this for initialization
	void Start () {
        //MobileControls = GameObject.FindGameObjectWithTag("MobileUI");

        //MobileControls.GetComponent<UIController>().CanvasOverride(true, false);

      //  StartFade();
	}

    public void StartFade()
    {
        fading = true;
        
        fadeCountdown = fadeClock;
        if (crossFade)
            fadeCountdown = fadeClock * 2;
     //   GetComponent<Canvas>().enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
        if (callFade)
        {
            StartFade();
            callFade = false;
        }

        if (fading == true)
        {
            if (!crossFade)
                blackness.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), fadeCountdown / fadeClock);
            else if (crossFade)
                blackness.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 1 - Mathf.Abs(fadeCountdown - fadeClock) / fadeClock);
            
            fadeCountdown -= Time.deltaTime;

            if ((!crossFade && fadeCountdown < 0.0f) || (crossFade && fadeCountdown < (0f - fadeClock)))
            {
     //           GetComponent<Canvas>().enabled = false;
           //     MobileControls.GetComponent<UIController>().CanvasOverride(false, true);
                fading = false;
            }
        }
	}
}
