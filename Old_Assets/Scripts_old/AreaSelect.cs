using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AreaSelect : MonoBehaviour {
	public int areaNumber = 0;
	public int mapToLoad = 01;
    public AreaSliderAlt selected;

	private bool readySelect = false;
	private bool readylocktwo = false;

	public Text scoreDisplay;
	public int areaScore = 0;
	public float areaProgress = 0;
    public int areaLives = 1;
    public bool loadControls = true;
    public bool loading = false;
    public Slider loadingBar;
    public Button startLoaded;
    public bool confirmWithButton = false; // pop up and wait for button click

    /*
    void Start()
    {
        loadingBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<Slider>();
        startLoaded.gameObject.SetActive(false);
    }
    */

	void Update()
	{

		if (selected.Area == areaNumber) {
			if (Input.GetButton ("Shift") || selected.launch == true) {
				MissionStarter ();
			}

			if (readySelect == true && readylocktwo == true) {
				MissionStarter ();
			} else {
				readySelect = false;
				readylocktwo = false;
			}
		}
            

		areaScore = Mathf.RoundToInt( PlayerPrefs.GetFloat (mapToLoad + " HighScore"));

		int objectives = PlayerPrefs.GetInt (mapToLoad + " TotalObjectives");
        if (objectives == 0)
            objectives = 1;
        int completed = PlayerPrefs.GetInt (mapToLoad + " ObjectivesComplete");



        areaLives = PlayerPrefs.GetInt(mapToLoad + " ShipsRemaining");

        areaProgress = Mathf.Round( 100.0f * (completed / objectives));

		scoreDisplay.text = "High Score: " + areaScore.ToString() + "\nProgress: " + areaProgress.ToString();
  
	}


	public void MissionStarter()
	{
//        SceneManager.LoadSceneAsync(selected.controlScene, LoadSceneMode.Single);
//        SceneManager.LoadSceneAsync(mapToLoad, LoadSceneMode.Additive);
        PlayerPrefs.SetInt("MapLoaded", mapToLoad);
        PlayerPrefs.SetInt("ControlMap", selected.controlScene);
        PlayerPrefs.SetInt("LoadingBarScene", selected.loadingBarScene);

        SceneManager.LoadScene(PlayerPrefs.GetInt("LoadingBarScene"));

        //StartCoroutine(AsynchronousLoad());

	}
		
    /*
    IEnumerator AsynchronousLoad ()
    {
        yield return null;
        AsyncOperation loadEnvironment = SceneManager.LoadSceneAsync(mapToLoad, LoadSceneMode.Single);


        loadEnvironment.allowSceneActivation = false;

//        AsyncOperation loadControlsMap = SceneManager.LoadSceneAsync(selected.controlScene, LoadSceneMode.Additive);
//        loadControlsMap.allowSceneActivation = false;



        while (!loadEnvironment.isDone)
        {
            // [0, 0.9] > [0, 1]
            float loadingProgress = Mathf.Clamp01(loadEnvironment.progress / 0.9f);

            if (loadEnvironment.progress < 0.9f)
            {
                loadingBar.value = loadingProgress;
            }

            //loading completed
            if (Mathf.Approximately(loadEnvironment.progress, 0.9f))
            {
                if (confirmWithButton == true)
                    startLoaded.gameObject.SetActive(true);

                if (startLoaded.GetComponent<startlevelButton>().StartLevel == true || confirmWithButton == false)
                {
                    SceneManager.LoadScene(PlayerPrefs.GetInt("ControlMap"), LoadSceneMode.Additive);
                    loadEnvironment.allowSceneActivation = true;
                }
            }

            yield return null;
        }


    }
    */


}
