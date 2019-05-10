using UnityEngine;
using System.Collections;

public class RedAlert : MonoBehaviour {
    public float pulseTime = 3f;
    private float t = 0f;
    private GameObject pulse; 
	// Use this for initialization
	void Start () {
        pulse = GetComponentInChildren<ParticleSystem>().gameObject;
        if (pulse != null)
            pulse.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (t > 0)
            t -= Time.deltaTime;
        else
            pulse.SetActive(false);
    }

    public void RedAlarm()
    {
        t = pulseTime;
        pulse.SetActive(true);
        
    }

}
