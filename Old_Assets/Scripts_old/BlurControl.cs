using UnityEngine;
using System.Collections;
//using UnityStandardAssets.ImageEffects;

public class BlurControl : MonoBehaviour {
    private static bool blurThis = false;
    public bool testBlur = false;

    public static bool pauseOverride = false;

    void LateUpdate ()
    {
        bool on = blurThis || pauseOverride;
     //   transform.GetComponent<Blur>().enabled = on;
        clear();
    }
    

    public static void blurry()
    {
        blurThis = true;
    }

    public static void clear()
    {
        blurThis = false;
    }
    
}
