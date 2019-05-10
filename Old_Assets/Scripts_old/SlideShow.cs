using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlideShow : MonoBehaviour {
	public bool stall = false;
	public GameObject[] slides;
	public int currentSlide = 0;
	public GameObject currentCamera;

    void Start()
    {
//        if (currentCamera == null)
//            currentCamera = GameObject.
    }

	void Update () {
		if (stall == true) 
		{
			Time.timeScale = 0.0f;
		}

//			currentCamera.transform.GetComponentInChildren<Blur>().enabled = true;

			
	}

	public void NextSlide()
	{
		currentSlide++;
		slideChanger ();
	}

	public void PreviousSlide()
	{
		currentSlide--;
		slideChanger ();
	}

	public void Escape()
	{
		Time.timeScale = 1.0f;
		gameObject.SetActive (false);
	}

	public void slideChanger()
	{
		if (currentSlide == slides.Length) 
		{
			Escape ();
		}

		for (int i = 0; i < slides.Length; i++) {
			if (i != currentSlide) {
				slides [i].SetActive (false);
			}

			slides [currentSlide].SetActive (true);
		}
					
	}

}