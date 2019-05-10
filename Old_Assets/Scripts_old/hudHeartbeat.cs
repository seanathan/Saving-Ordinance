using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hudHeartbeat : MonoBehaviour {
    public float pulseTime = 1.0f;
    public float pulseMax = 0.4f;
    public float pulseMin = 0.1f;
    private bool waning = false;
    private float t = 0;
    public bool HPAlarm = false;
    public float alarmPulse = 2.0f;
    public bool wavePulse = false;
    public float waveWidth = 1000f;

    void Start()
    {
        t = 0 - Mathf.Abs((transform.position.x - (0.5f * waveWidth)) / waveWidth);
    }

	// Update is called once per frame
	void Update () {
        Color init = GetComponent<Image>().color;

        float pulseUpper = pulseMax;
        float alarming = 1.0f;

        if (PlayerControls.getPlayerShip() == null)
            return;



        if (HPAlarm && (PlayerControls.getPlayerShip().getHealth().ratio < 0.2f))
        {
            alarming = alarmPulse;
            pulseUpper = pulseMax * 2;
        }
        init.a = Mathf.Lerp(pulseMin, pulseUpper, t);
        GetComponent<Image>().color = init;

        if (!waning)
        {
            t += pulseTime * alarming * Time.deltaTime;
            if (t > 1.0f)
                waning = true;
        }
        else 
        {
            t -= pulseTime * alarming * Time.deltaTime;
            if (t < 0.0f)
                waning = false;
        }

	}
}
