using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class throttleReset : MonoBehaviour {

    public bool killThrottleOnLanding = false;

    // Update is called once per frame
    public void ThrottleReset () {
        if (killThrottleOnLanding)
            GetComponent<Slider>().value = 0;

	}
}
