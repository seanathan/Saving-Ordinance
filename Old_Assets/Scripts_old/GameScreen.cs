using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour {
    public bool gameScreenOn = false;

#if UNITY_EDITOR
    void OnValidate()
    {
        Recap();
    }
#endif
	// Update is called once per frame
	void Update () {
        Recap();
        
        //Debug.Log(string.Format("This is {0}", gameObject.name));
    }

    public void Recap()
    {
        GetComponent<Canvas>().enabled = gameScreenOn;

        if (gameScreenOn)
        {
            BlurControl.blurry();
            UIController.suspendUI = true;

            GetComponentInChildren<SoulWrapup>().EndSoulCount();
        }
    }
}
