using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class crashText : MonoBehaviour {
    public Text output;
    public static string crashMessage;
    private float resetTimer = 2.0f;
    private float resetcountdown = 0.0f;
    private string last;
    public float scrollSpeed = 1;
    public float initZ;


	// Use this for initialization
	void Start () {
        output = transform.GetComponent<Text>();
        output.text = "  ";
        initZ = output.rectTransform.localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
        output.text = crashMessage;

        if (crashMessage != last && last == "  ")
        {
            resetcountdown = resetTimer;
        }

        if (resetcountdown < 0.5f)
        {
            resetcountdown = 0.0f;
        }
        if (resetcountdown > 0.0f)
        {
            resetcountdown -= Time.deltaTime;
        }
        float newZ = initZ + scrollSpeed * (1.0f - (resetcountdown / resetTimer));
        last = crashMessage;
        output.rectTransform.localPosition = new Vector3(output.rectTransform.localPosition.x, output.rectTransform.localPosition.y, newZ);
	}
}
