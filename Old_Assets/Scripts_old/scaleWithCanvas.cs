using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scaleWithCanvas : MonoBehaviour {
    public GameObject mainCanvas;
    public GameObject canvElement;
    public float distance = 0f;
    public float scalar = 0f;
    public float corrector = 10f;
    public float refRatio = 1.333333f;
    public float curRatio = 1f;
    public float curRef;
    public float adjustedScalar;
    private Camera mainCam;
    public GameObject[] children;



    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void ScaleAdjust()
    {
        curRatio = mainCam.aspect;
        refRatio = mainCanvas.GetComponent<CanvasScaler>().referenceResolution.x /
                            mainCanvas.GetComponent<CanvasScaler>().referenceResolution.y;

        curRef = curRatio / refRatio;
        adjustedScalar = (((curRatio - refRatio) * corrector) + refRatio ) / refRatio;
        transform.localScale = Vector3.one * adjustedScalar;
        
        //transform.localScale = Vector3.one * curRatio/refRatio;
        // transform.localScale = Vector3.one * mainCanvas.GetComponent<CanvasScaler>().scaleFactor;
        //1 at 800/600 (1.33)
        //1.36 at 2560/1440 (1.7777)
    }

    // Update is called once per frame
    void Update () {
        ScaleAdjust();
        
                    
	}

    void LateUpdate()
    {
        Suppress();
    }

    public void Suppress()
    {
        foreach (GameObject thing in children)
            thing.SetActive(ScoreKeeper.mobile && !UIController.suspendUI);

    }
}
