using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class actorImage : MonoBehaviour {
    

	// Update is called once per frame
	void Update () {
        GetComponent<RawImage>().enabled = (NarrationWriter.actor == gameObject.name);

    }
}
