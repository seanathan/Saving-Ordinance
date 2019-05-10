using UnityEngine;
using System.Collections;

public class getCleaned : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (cleanup.cleaner != null)
            transform.SetParent(cleanup.cleaner.transform);
    }
	
	// Update is called once per frame
	void Update () {

        if (cleanup.cleaner != null)
            transform.SetParent(cleanup.cleaner.transform);
	}
}
