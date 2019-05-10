using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class FlightStick : MonoBehaviour {

	public int fingerTouched;
	public Text corners;
	public Vector2 touchStart;
	public Vector2 touchDragged;
	public Vector2 totalDrag;
	public int resX;
	public int resY;
	public float dragScalar = 5.0f;
	public float xMove;
	public float yMove;
	public int fingerTouchedID;
	private bool sameFinger;


	// Use this for initialization
	void Start () {

		resX = Screen.width;
		resY = Screen.height;
		JoystickRelease ();
		fingerTouched = 0;

	}
		
	public void JoystickAquire()
	{
		
		Touch[] findTouch = Input.touches;
		for (int i = 0; i < Input.touchCount; i++) 
		{
			if (Input.touches [i].fingerId == fingerTouchedID)
			{
				sameFinger = true;


			}
		}
		if (sameFinger == false)
		{
		for (int i = 0; i < Input.touchCount; i++) 
				{
			if (findTouch [i].phase == TouchPhase.Began) 
			{
				
					
				fingerTouched = i;
				fingerTouchedID = Input.touches [fingerTouched].fingerId;
				}

			}
		}
	}

	public void JoystickTouch()
	{
		if (sameFinger == false)
		{
		Touch[] findTouch = Input.touches;
		for (int i = 0; i < Input.touchCount; i++) {
			if (findTouch [i].fingerId == fingerTouchedID) {
				fingerTouched = i;
			}
		}
			
		touchStart = Input.touches[fingerTouched].position;
		resX = Screen.width;
		resY = Screen.height;
			sameFinger = true;
		}
	}

	public void JoystickDrag()
	{
		Touch[] findTouch = Input.touches;
		for (int i = 0; i < Input.touchCount; i++) {
			if (findTouch [i].fingerId == fingerTouchedID) {
				fingerTouched = i;
			}
		}
		touchDragged = Input.touches[fingerTouched].position;
		totalDrag = touchDragged - touchStart;

		xMove = Mathf.Clamp ((totalDrag.x / resX) * dragScalar, -1.0f, 1.0f);
		yMove = Mathf.Clamp ((-totalDrag.y / resY) * dragScalar, -1.0f, 1.0f);
		corners.text = xMove.ToString () + ", " + yMove.ToString ();
	}

	public void JoystickRelease()
	{
		//reset
		totalDrag = Vector2.zero;
		yMove = 0.0f;
		xMove = 0.0f;
		sameFinger = false;
	}

		


}
