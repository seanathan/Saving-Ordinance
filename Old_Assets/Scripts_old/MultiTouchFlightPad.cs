using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MultiTouchFlightPad : MonoBehaviour {

    //To reference this script in a player controller, simply assign a VirtualPointerStick and reference the "position" component of the script (stored as vector3, but only the x and y will be relevant).
    //Position is clamped to within -1 and 1

    //first stick
    public bool engaged = false;
    public Vector3 position;
    private Vector3 initPosition;
    private Vector3 restPosition;
    public float deadZone = 0.1f;


    //How sensitive would you like the input touch? I set 100 pixels per 1 unit of output as standard.
    public float sensitivity = 150;

    //How many pixels would you like your stick to be able to move? 50 has worked well for me
    public float stickWiggle = 50;

    public int flightTouchNum = -1;
    public int yInvert = -1;

    public GameObject flightStickUI;
    public GameObject fireStickUI;
    public bool touchpad = true;

    //tap to fire
    public int fireTouchNum = -1;
    public bool fireEngaged = false;

    public float resetter = 1f;

    /// <summary>
    /// Do-over code
    /// 
    /// on first finger down: (will always be first finger)
    ///     1) initPosition = touch[0].position
    ///     2) move the icon to the position of touch[0]
    ///     
    /// on second finger down:
    ///     
    /// 
    /// </summary>

    void Start()
    {
        flightTouchNum = -1;
        fireTouchNum = -1;
        
        engaged = false;
        fireEngaged = false;

        if (!flightStickUI)
            flightStickUI = new GameObject("UI Null");

        if (!fireStickUI)
            fireStickUI = new GameObject("UI Null");

    }

    void Update()
    {

        //error catch
        if (fireTouchNum == flightTouchNum)
            fireTouchNum = -1;
        
        //clear touch nums
        if (!engaged)
            flightTouchNum = -1;

        if (!fireEngaged)
            fireTouchNum = -1;

        //RELEASE

        if (engaged && Input.GetMouseButtonUp(0))
        {
            engaged = false;
        }
        
        engaged = isEngaged(engaged, flightTouchNum);
        fireEngaged = isEngaged(fireEngaged, fireTouchNum);


        //set active child marker
        flightStickUI.SetActive(engaged);
        
        fireStickUI.SetActive(fireEngaged);

        /////

        //TRACK
        if (engaged)
        {
            position = (TouchPosition(flightTouchNum) - initPosition) / (sensitivity * Screen.width / GetComponentInParent<CanvasScaler>().referenceResolution.x);
            
            position.x = Mathf.Clamp(position.x, -1, 1);

            position.y = yInvert * Mathf.Clamp(position.y, -1, 1);

            if (gotTapped(flightTouchNum))
            {
                GetComponentInParent<MobileUI>().HoverMomentary(true);
            }
            
            //DEADZONE
            if (Mathf.Abs(position.x) < deadZone)
                position.x = 0.0f;

            if (Mathf.Abs(position.y) < deadZone)
                position.y = 0.0f;
        }
        
        //fireTrigger
        MobileUI TouchControls = GameObject.FindGameObjectWithTag("MobileUI").GetComponent<MobileUI>();
        
        TouchControls.Trigger(fireEngaged);
        
        if (!engaged)
        {
            position = Vector3.MoveTowards(position, Vector3.zero, resetter * Time.deltaTime);
        }

        //where should the stick image be? Set wiggle to zero if you want it to stay still.
        flightStickUI.transform.position = new Vector3(initPosition.x + (position.x * stickWiggle), initPosition.y + (yInvert * position.y * stickWiggle), 0.0f);
    }

    bool gotTapped(int fingerID)
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == fingerID)
            {
                return (touch.tapCount == 2);
            }
        }

        //pointer clicks?

        return false;    //returns started bool value, no change
    }

    int WhoStarted()
    {
        int fingerNum = -1;  //returns -1 if not found

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerNum = touch.fingerId;
            }

        }
        
        return fingerNum;
    }

    Vector3 TouchPosition(int fingerID)
    {
        if (fingerID == -1)
            return Input.mousePosition;

        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == fingerID)
            {
                return touch.position;
            }
        }

        

        return Vector3.zero;
    }

    bool isEngaged(bool whichEngaged, int fingerID)
    {
        
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == fingerID)
            {
                    return (touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended);
               
            }
        }

        return whichEngaged;    //returns started bool value, no change
    }

    void stickSet()
    {
        position = Vector3.zero;

        if (!engaged)
        {
            flightTouchNum = WhoStarted();
            
            initPosition = TouchPosition(flightTouchNum);

            flightStickUI.transform.position = initPosition;
            engaged = true;
        }
        else if (!fireEngaged && Input.touchCount > 1)
        {
            fireTouchNum = WhoStarted();
            
            fireStickUI.transform.position = TouchPosition(fireTouchNum);   //place the fire icon under the finger, locked to position

            fireEngaged = true;
        }

    }
    
}

