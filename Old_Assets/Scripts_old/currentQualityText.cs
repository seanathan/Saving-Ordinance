using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class currentQualityText : MonoBehaviour {
    public int qLevel;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        qLevel = QualitySettings.GetQualityLevel();
        transform.GetComponent<Text>().text = qLevel + " " + QualitySettings.names[qLevel].ToString();
    }
}
