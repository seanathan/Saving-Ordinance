using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class currentUIText : MonoBehaviour {
    public ScoreKeeper sc;
    public int LastUiType = 0;
    private int uiCurrent;
    
    // Use this for initialization
    void OnValidate()
    {
        sc = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
    }

    void Awake()
    {
        LastUiType = 0;
        LastUiType = PlayerPrefs.GetInt("LastUiType");


        GetComponentInParent<Slider>().maxValue = sc.statusUI.Sets.Length -1;

        GetComponentInParent<Slider>().value = LastUiType;

        sc.UiStyle(LastUiType);
    }

    // Update is called once per frame
    void Update()
    {
        uiCurrent = Mathf.RoundToInt(GetComponentInParent<Slider>().value);
        
        GetComponent<Text>().text = "Status Bars:\n\n" + sc.statusUI.currentSet;
    }
}
