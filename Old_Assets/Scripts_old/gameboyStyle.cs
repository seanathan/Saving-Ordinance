using UnityEngine;
using System.Collections;

public class gameboyStyle : MonoBehaviour {
	public float landscapeY;
	public float shiftY;


	void Update () {
		if (Screen.orientation == ScreenOrientation.Portrait) 
		{
			transform.localPosition = new Vector3 (transform.localPosition.x, landscapeY + shiftY, transform.localPosition.z);
		}
		else 
		{
			transform.localPosition = new Vector3 (transform.localPosition.x, landscapeY, transform.localPosition.z);
		}
	}

	//transform.GetComponent<RectTransform>().localPosition
}
