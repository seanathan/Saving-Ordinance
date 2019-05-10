using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class switchMenu : MonoBehaviour {

	public Canvas menuOff;
	public Canvas menuOn;

	void Update()
	{
		if (menuOff.enabled) {
			if (Input.GetButton ("Shift")) {
				MenuSwap ();
			}
		}
	}

	// Update is called once per frame


	public void MenuSwap()
	{
		menuOn.enabled = true;
		menuOff.enabled = false;
	}
}

