using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class speedSlider : MonoBehaviour {
    private Slider myslider;
    public float threshold = 350;
    public bool negative = false;

    public Image fillbar;
    public Color max = new Color32(255, 255, 255, 178);
    public Color min = new Color32(0, 255, 0, 178);

    // Update is called once per frame
    void Update () {
        myslider = GetComponent<Slider>();

        myslider.value = ScoreKeeper.playerSpeed / threshold;
        
        if (fillbar != null)
            fillbar.color = Color.Lerp(min, max, Mathf.Pow( myslider.value, 2));
	}
}
