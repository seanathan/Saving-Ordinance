using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class moodEdge : MonoBehaviour {

    public Color fiero;
    public Color anxiety;
    public Color relief;
    public Color remorse;
    public Color neutral;
    public Color current;
    private static moodEdge moods;
    
    public float pulseTime = 1.0f;
    public float pulseMax = 0.4f;
    public float pulseMin = 0.1f;
    private bool waning = false;
    private float t = 0;
    public float scalar = 15f;

    public bool heartbeat = true;

    void Awake()
    {
        moods = this;
    }

    public static moodEdge getMoods()
    {
        if (moods == null)
            moods = GameObject.FindObjectOfType<moodEdge>();

        return moods;
    }


    
    void Start()
    {
        moods = this;

        GetComponent<Image>().color = neutral;
    }

    // Update is called once per frame
    void Update()
    {
        if (moods == null)
            moods = this;

        heartbeat = (moods.GetComponent<Image>().color != neutral);
        if (heartbeat)
        {
            Color init = GetComponent<Image>().color;

            float pulseUpper = pulseMax;

            init.a = Mathf.Lerp(pulseMin, pulseUpper, t);
            GetComponent<Image>().color = init;
            
            if (!waning)
            {
                t += pulseTime * Time.deltaTime;
                if (t > 1.0f)
                    waning = true;
            }
            else
            {
                t -= pulseTime * Time.deltaTime;
                if (t < 0f)
                    waning = false;
            }

           // if (t < 0)
            //    MoodSwitch(moods.neutral);
        }

    }

    public static void MoodSwitch(Color moodColor)
    {
        if (getMoods() == null)
            return;

        moods.current = moodColor;
        moods.GetComponent<Image>().color = moods.current;
        moods.t = 0;
        moods.waning = false;
        
	}
}
