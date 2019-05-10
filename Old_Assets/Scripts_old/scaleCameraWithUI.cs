using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scaleCameraWithUI : MonoBehaviour {
    public Vector2 camSize;
    public GameObject insetCamera;
    public GameObject refImage;
	// Use this for initialization
	void Resize () {
        Rect r = refImage.GetComponent<RectTransform>().rect;
        insetCamera.GetComponent<Camera>().pixelRect = r;
        insetCamera.transform.localScale = refImage.transform.lossyScale;
    }

    void Resize2()
    {

        Rect r = insetCamera.GetComponent<Camera>().pixelRect;
        r.size = refImage.GetComponent<RectTransform>().rect.size * refImage.transform.lossyScale.x;
        r.position = refImage.transform.localPosition * refImage.transform.lossyScale.x;
        //r.size = camSize;
        camSize = refImage.transform.position;
        insetCamera.GetComponent<Camera>().pixelRect = r;
    }

	// Update is called once per frame
	void Update () {
        Resize2();


    }

    void OnValidate()
    {
        Resize2();
    }
}

