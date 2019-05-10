using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class starsBarDisplay : MonoBehaviour {


	// Update is called once per frame
	void Update () {
        GetComponent<Image>().fillAmount = ScoreKeeper.starScore/3f;
	}
}
