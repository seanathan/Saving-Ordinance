using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThreatHP : MonoBehaviour {


	public Slider hpbar;
	public Text shipStatus;
	public Canvas uiCanvas;
	public Image hpcolor;

    [Multiline]
    public string statusFormat = "{0} {1} {2}";

    void Start()
    {
        Init();
    }

    void OnValidate()
    {
        Init();
    }

    void Init()
    {
        if (shipStatus)
            statusText();

        if (!hpbar)
            hpbar = GetComponentInChildren<Slider>();
        if (!shipStatus)
            shipStatus = GetComponentInChildren<Text>();

        if (!uiCanvas)
            uiCanvas = GetComponentInParent<Canvas>();
        if (!hpcolor)
            hpcolor = hpbar.image;
    }

    public void statusText(string statusMessage = "Ship Status", string IFFmessage = "Ship Name", string mobHullMessage = "100/100")
    {
        shipStatus.text = string.Format(statusFormat, statusMessage, IFFmessage, mobHullMessage);
    }
}
