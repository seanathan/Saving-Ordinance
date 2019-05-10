using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualPointerStick : MonoBehaviour {
    /*


    //To reference this script in a player controller, simply assign a VirtualPointerStick and reference the "position" component of the script (stored as vector3, but only the x and y will be relevant).
    //Position is clamped to within -1 and 1

    //first stick
    public bool engaged = false;
    public Vector3 position;
    private Vector3 initPosition;
    private Vector3 restPosition;
	public float deadZone = 0.1f;


    //How sensitive would you like the input touch? I set 100 pixels per 1 unit of output as standard.
    public float sensitivity = 100;
    
    //How many pixels would you like your stick to be able to move? 50 has worked well for me
    public float stickWiggle = 50;

    public int touchNum = -10;
	public int yInvert = -1;

	public GameObject childStick;
	public bool touchpad = false;

    //tap to fire
    public int fireTouch = 0;
    public bool fireEngaged = false;

    void Start()

    {
        //gathers the initial position of the thumbstick
        touchNum = -10;
        restPosition = transform.GetComponent<RectTransform>().localPosition;
        engaged = false;
        fireEngaged = false;
    }


 
    public void stickSet()
    {

        //call this with a UI element (image, rawImage, button, doesn't matter) on both "Begin Drag" and "Pointer Down" events 
        //(If you find one works better than the other, go for it, but you can always be better safe than sorry)


        position = Vector3.zero;


        engaged = true;
        if (Input.touchCount > 0 && touchNum == -1)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchNum = touch.fingerId;

                }
                //hover??
                if (touch.tapCount == 2)
                {
                    //ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().hover = 1.0f;
                    GetComponentInParent<MobileUI>().HoverMomentary(true);
                }
                //touchNum = Input.GetTouch(touchNum).fingerId
            }
            initPosition = Input.GetTouch(touchNum).position;

            Debug.Log(gameObject.name.ToString() + " finger ID is " + touchNum);

        }

        //fire control
        else if (Input.touchCount > 1 && touchNum != -10 && fireEngaged == false)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fireTouch = touch.fingerId;
                    fireEngaged = true;
                }

                //touchNum = Input.GetTouch(touchNum).fingerId
            }
           // initPosition = Input.GetTouch(touchNum).position;

           // Debug.Log(gameObject.name.ToString() + " finger ID is " + touchNum);

        }
        if (Input.touchCount == 0)
        {
			initPosition = Input.mousePosition;

        }

        if (Input.touchCount > 0 )
            if (touchpad && Input.GetTouch(touchNum).phase == TouchPhase.Began) 
            {
    			childStick.SetActive(true);
    			childStick.GetComponent<RectTransform> ().position = initPosition;
    		}

			//if TOUCHPAD, activate child marker, set active, bring to touch init position

    }


    public void pStick()
    {
        //Call this function on your UI element with a "Drag" event
        if (Input.touchCount > 0)
        {
//            int actionFinger = -10;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].fingerId == touchNum)
                {
                    //actionFinger = i;
                    position.x = Mathf.Clamp(((Input.GetTouch(i).position.x - initPosition.x) / (sensitivity * Screen.width / 800.0f)), -1, 1);

                    position.y = yInvert * Mathf.Clamp(((Input.GetTouch(i).position.y - initPosition.y) / (sensitivity * Screen.width / 800.0f)), -1, 1);
                }
                if (Input.touches[i].tapCount == 2)
                {
                    //ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().hover = 1.0f;
                    GetComponentInParent<MobileUI>().HoverMomentary(1.0f);
                }
            }
        }

        if (Input.touchCount == 0)
        {
			position.x = Mathf.Clamp(((Input.mousePosition.x - initPosition.x) / (sensitivity * Screen.width /800.0f)), -1.0f, 1.0f);

			position.y = yInvert * Mathf.Clamp(((Input.mousePosition.y - initPosition.y) / (sensitivity * Screen.width /800.0f)), -1.0f, 1.0f);
        }

		if (Mathf.Abs( position.x ) < deadZone)
			position.x = 0.0f;

		if (Mathf.Abs( position.y ) < deadZone) 
			position.y = 0.0f;
    }

    public void stickRelease()
    { 
        if (Input.touchCount > 0 && Input.GetTouch(touchNum).phase == TouchPhase.Ended)
        {
            position = Vector3.zero;
            if (touchNum != -10)
            {
                Debug.Log(gameObject.name.ToString() + " finger ID " + touchNum + " released");
                engaged = false;
                touchNum = -10;

                ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().hover = 0.0f;

            }
        }
        else if (Input.touchCount == 0)
        {
            engaged = false;
            position = Vector3.zero;
        }

		//if TOUCHPAD, set inactive child marker
		if (touchpad)
			childStick.SetActive(false);
    }

    /*
    void Update()
    {
        if (touchNum != -10 && Input.GetTouch(touchNum).phase == TouchPhase.Ended)
        {
            position = Vector3.zero;
            if (touchNum != -10)
            {
                Debug.Log(gameObject.name.ToString() + " finger ID " + touchNum + " released");
                engaged = false;
                touchNum = -10;

                ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().hover = 0.0f;

            }
        }
        else if (Input.touchCount == 0)
        {
            engaged = false;
            position = Vector3.zero;
        }

        //if TOUCHPAD, set inactive child marker
        if (touchpad)
            childStick.SetActive(false);
    }

    void LateUpdate()
    {
        if (fireEngaged == true)
        {
            MobileUI TouchControls = GameObject.FindGameObjectWithTag("MobileUI").GetComponent<MobileUI>();
            TouchControls.Trigger(true);
            if (Input.GetTouch(fireTouch).phase == TouchPhase.Ended || Input.GetTouch(fireTouch).phase == TouchPhase.Canceled)
            {
                fireEngaged = false;
                TouchControls.Trigger(false);
            }
        }
        //where should the stick image be? Set wiggle to zero if you want it to stay still.
        transform.GetComponent<RectTransform>().localPosition = new Vector3(restPosition.x + (position.x * stickWiggle), restPosition.y + (yInvert * position.y * stickWiggle), 0.0f);
    }

    */

}
