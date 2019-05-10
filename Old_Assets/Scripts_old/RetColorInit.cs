using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RetColorInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
