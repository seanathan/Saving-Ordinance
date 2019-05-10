using UnityEngine;
using System.Collections;

public class viewAdjuster : MonoBehaviour {

	public GunCamera mainCamera;
	public int[] locks = { -9, 0, 6 };
	public int currentView = 2;
	public float zoomSpeed = 3.0f;

    void Awake()
    {
        mainCamera = gameObject.GetComponentInParent<MobileUI>().MainGunCamera;
    }


    void Start()
    {
        if (mainCamera == null)
            mainCamera = gameObject.GetComponentInParent<MobileUI>().MainGunCamera;
    }

	void Update()
	{
		mainCamera.wheelPos = Mathf.Lerp(mainCamera.wheelPos, locks [currentView], Time.deltaTime * zoomSpeed);
	}

	public void ButtonPress()
	{
		currentView++;
		if (currentView > 2) 
		{
			currentView = 0;
		}


	}

}
