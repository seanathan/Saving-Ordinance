using UnityEngine;
using System.Collections;
//using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {


	private GameObject mainCamera;
	public static int pitchInvert = 1;
	public int currentInv;
    public SelectFromSets MenuSelector;

	void Awake()
	{
        
		if (PlayerPrefs.GetInt ("Pitch Invert") == 0) 
		{
			PlayerPrefs.SetInt ("Pitch Invert", pitchInvert);
		}
	}

	void Start()
	{
		pitchInvert = PlayerPrefs.GetInt("Pitch Invert");
        mainCamera = GameObject.FindGameObjectWithTag("CameraControl");

    }

    void OnValidate()
    {
        MenuSelector = GetComponent<SelectFromSets>();
    }



    // Update is called once per frame
    void Update () {
        GetComponent<Canvas>().enabled = Pause.paused;  //enable canvas when paused

        BlurControl.pauseOverride = (Pause.paused);

        currentInv = pitchInvert;
	}

	public void Pitch ()
	{
		if (pitchInvert == -1) {
			pitchInvert = 1;
		}
		else if (pitchInvert == 1) {
			pitchInvert = -1;
		}
		PlayerPrefs.SetInt("Pitch Invert", pitchInvert);
	}

	public void Reset ()
	{
		Time.timeScale = 1.0f;

        ScoreKeeper.LeaveGame();
    }

    
    public void nextMenu()
    {
        MenuSelector.nextSet();

    }

    public void prevMenu()
    {
        MenuSelector.prevSet();
    }

    public void menuTab(int tab)
    {
        MenuSelector.SetSelect(tab);
    }
}

