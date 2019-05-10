using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tapCounter : MonoBehaviour {

    
    // Update is called once per frame
	void Update ()
    {
        if (Input.touchCount > 0)
        {
            transform.GetComponent<Text>().text = " ";

            foreach (Touch touch in Input.touches)
            {
                transform.GetComponent<Text>().text += "Touch " + touch.fingerId.ToString() + " " + touch.position.ToString() + "\n";
                transform.GetComponent<Text>().text += "Touch " + touch.fingerId.ToString() + " " + touch.phase.ToString() + "\n";
                //transform.GetComponent<Text>().text += "Touch " + touch.fingerId.ToString() + " " + touch.radius.ToString() + "\n";
                transform.GetComponent<Text>().text += "Touch " + touch.fingerId.ToString() + " " + touch.tapCount.ToString() + "\n";
            }


        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            transform.GetComponent<Text>().text = " ";
            transform.GetComponent<Text>().text += "Mouse " + Input.mousePosition.ToString() + "\n";
        }
        else
        {
            transform.GetComponent<Text>().text = " ";
        }
    }
}
