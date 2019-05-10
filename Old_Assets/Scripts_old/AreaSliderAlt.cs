using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AreaSliderAlt : MonoBehaviour {


    //To reference this script in a player controller, simply assign a VirtualPointerStick and reference the "position" component of the script (stored as vector3, but only the x and y will be relevant).
    //Position is clamped to within -1 and 1

    public bool engaged = false;
    public Vector3 position;
    private Vector3 initPosition;
    private Vector3 restPosition;
    public float deltaDrag;
    public float totalDrag;
    public float startValue =2;
    public float dragGoal = 0;
    public float sliderWidth = 2000.0f;
    public int elementCount = 5;
    public float coastSpeed = 10.0f;
    public int Area;
    public float dragSpeed = 1.0f;
    public float deadZone = 0.1f;
    public bool launch = false;
    public float ElementValue;
    public int controlScene = 02;
    public int loadingBarScene = 01;
    //
    //    public float clickTime = 0.2f;
    //    public float clickTimer = 0.0f;

    public int touchNum = 0;

    // Use this for initialization
    void Start () 
    {
        restPosition = transform.GetComponent<RectTransform>().localPosition;
        engaged = false;
        totalDrag = 0.0f;
        deltaDrag = 0.0f;
        //startValue = Mathf.Round(ElementValue);


    }

    //    public void DownClick()
    //    {
    //        clickTimer = clickTime;
    //    }
    //
    public void UpClick()
    {
        if (engaged == false)
        {
            launch = true;
        }
    }

    void LateUpdate()
    {
        
        //        clickTimer -= Time.deltaTime;
        //must be applied to Slider tool
        //Value of area will be inverse of 

        if (engaged == false)
        {

            totalDrag = Mathf.Lerp(totalDrag, dragGoal, coastSpeed * Time.deltaTime);
        }

        ElementValue = startValue + (totalDrag / (((sliderWidth / 800) * Screen.width)/ elementCount));

        ElementValue = Mathf.Clamp(ElementValue, 0, elementCount - 1);
        Area = elementCount - Mathf.RoundToInt(ElementValue);
        float unit = sliderWidth / (elementCount - 1);
        float firstPos = sliderWidth / 2.0f;
        GetComponent<RectTransform>().localPosition = new Vector3 (ElementValue * unit - firstPos, 0.0f, 0.0f);

        //slider 0 = -1000
        //slider 2 = 0
        //slider 4 = 1000

        //slider 0 = (
    }

    public void stickSet()
    {

        //call this with a UI element (image, rawImage, button, doesn't matter) on both "Begin Drag" and "Pointer Down" events 
        //(If you find one works better than the other, go for it, but you can always be better safe than sorry)


        position = Vector3.zero;


        engaged = true;
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Moved)

                    touchNum = touch.fingerId;
            }

            touchNum = Input.GetTouch(touchNum).fingerId;

            initPosition = Input.GetTouch(touchNum).position;
        }
        if (Input.touchCount == 0)
        {
            initPosition = Input.mousePosition;
        }


    }


    public void pStick()
    {

        //Call this function on your UI element with a "Drag" event
        float lastDrag = position.x;

        if (Input.touchCount > 0)
        {
            position.x = Input.GetTouch(touchNum).position.x - initPosition.x;

        }
        if (Input.touchCount == 0)
        {
            position.x = Input.mousePosition.x - initPosition.x;
        }

        if (Mathf.Abs( position.x ) < deadZone) 
        {
            position.x = 0.0f;
        }

        deltaDrag = position.x - lastDrag;
        //reset total drag each frame
        totalDrag += deltaDrag * dragSpeed;
    }

    public void stickRelease()
    { 
        position = Vector3.zero;
        engaged = false;
        //coast
        dragGoal = (((sliderWidth / 800) * Screen.width)/ elementCount) * Mathf.Round (ElementValue - startValue);
    }

    public void launcher()
    {
        launch = true;
    }

}
