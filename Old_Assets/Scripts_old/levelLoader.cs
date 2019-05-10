using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelLoader : MonoBehaviour {
    
    public int mapToLoad = 03;
    public int controlScene = 02;
    public int loadingBarScene = 01;


    public Slider loadingBar;
    public Button startLoaded;
    public bool confirmWithButton = false; // pop up and wait for button click

    // Use this for initialization
    void Start () {
        startLoaded.gameObject.SetActive(false);

        mapToLoad = PlayerPrefs.GetInt("MapLoaded");
        controlScene = PlayerPrefs.GetInt("ControlMap");
        loadingBarScene = PlayerPrefs.GetInt("LoadingBarScene");
        MissionStarter();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MissionStarter()
    {
        //        SceneManager.LoadSceneAsync(selected.controlScene, LoadSceneMode.Single);
        //        SceneManager.LoadSceneAsync(mapToLoad, LoadSceneMode.Additive);
        //PlayerPrefs.SetInt("MapLoaded", mapToLoad);

        

        StartCoroutine(AsynchronousLoad());

    }

    IEnumerator AsynchronousLoad()
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
                if (confirmWithButton)
                    startLoaded.gameObject.SetActive(true);

                if (startLoaded.GetComponent<startlevelButton>().StartLevel || !confirmWithButton)
                {

                    //ENGAGE

                    SceneManager.LoadScene(controlScene, LoadSceneMode.Additive);
                    loadEnvironment.allowSceneActivation = true;
                   // SceneManager.UnloadScene(loadingBarScene);
                }
            }

            yield return null;
        }

        
    }

    public void EngageMap()
    {


    }
}
