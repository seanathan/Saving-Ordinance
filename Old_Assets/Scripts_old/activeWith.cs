using UnityEngine;
using System.Collections;

public class activeWith : MonoBehaviour {

    public GameObject[] thingsActive;
	
	// Update is called once per frame
	void OnDisable() {
        for (int i = 0; i < thingsActive.Length; i++)
        {

            thingsActive[i].SetActive(false);
        }
	}

    void OnEnable()
    {
        for (int i = 0; i < thingsActive.Length; i++)
        {

            thingsActive[i].SetActive(true);
        }
    }
}
