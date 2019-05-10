using UnityEngine;
using System.Collections;

public class ShipFlightControls : MonoBehaviour
{
	public float moveVertical = 0.0f;
	public float moveHorizontal = 0.0f;
	public float hoverVertical = 0.0f;
	public float hoverHorizontal = 0.0f;

	public float thrust = 0f;
	public float rollx = 0f;
	public float rolly = 0f;
	
	public void setSticks(float mV, float mH, float hV, float hH){
		moveHorizontal = mH;
		moveVertical = mV;

		hoverHorizontal = hH;
		hoverVertical = hV;
	}
	

}
