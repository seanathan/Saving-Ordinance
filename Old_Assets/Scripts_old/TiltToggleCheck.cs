using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TiltToggleCheck : MonoBehaviour {
	public Color off = new Color32( 255, 255, 255, 178 );
	public Color on = new Color32( 0, 255, 0, 178 );

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ColorBlock colorEdit = gameObject.GetComponent<Button> ().colors;

        if (PlayerControls.GetActivePlayer() == null)
            return;

		if (PlayerControls.GetActivePlayer().tiltControl)
		{
			colorEdit.normalColor = on;
			colorEdit.highlightedColor = on;
			gameObject.GetComponent<Button> ().colors = colorEdit;


		}
		else
		{
			colorEdit.normalColor = off;
			colorEdit.highlightedColor = off;
			gameObject.GetComponent<Button> ().colors = colorEdit;
		}
	}
}
