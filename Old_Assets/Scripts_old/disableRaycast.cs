using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class disableRaycast : MonoBehaviour {
    private bool init;

	void Start () {
        init = GetComponent<GraphicRaycaster>().enabled;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Canvas>() != null)
        {

            if (GetComponent<Canvas>().enabled)
                GetComponent<GraphicRaycaster>().enabled = init;
            else
                GetComponent<GraphicRaycaster>().enabled = false;
        }

    }
}
