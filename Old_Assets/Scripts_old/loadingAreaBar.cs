using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadingAreaBar : MonoBehaviour {
    public AreaSelect loader;
    public float progress = 0.0f;
    public int mapToLoad;

    // Use this for initialization
	void Start () {

	
	}
	

    IEnumerator AsynchronousLoad ()
    {
        yield return null;

        //AsyncOperation loadControls = SceneManager.LoadSceneAsync(selected.controlScene, LoadSceneMode.Single);
        //loadControls.allowSceneActivation = false;


        AsyncOperation loadEnvironment = SceneManager.LoadSceneAsync(mapToLoad, LoadSceneMode.Single);
        loadEnvironment.allowSceneActivation = false;

        while (!loadEnvironment.isDone)
            {
                // [0, 0.9] > [0, 1]
                 progress = Mathf.Clamp01(loadEnvironment.progress / 0.9f);
                Debug.Log("Loading progress: " + (progress * 100) + "%");
    
                //loading completed
                if (loadEnvironment.progress == 0.9f)
                {
                    Debug.Log("Click anywhere to start");
                    if (Input.touchCount > 0 || Input.GetMouseButton(0))
                    {
                        loadEnvironment.allowSceneActivation = true;
                        //loadControls.allowSceneActivation = true;
                    }
                }
    
                yield return null;
            }
    
    
        }
}
