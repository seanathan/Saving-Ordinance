using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class CombineUIandArea : MonoBehaviour {

//    public string controls;
    public string environment;

	// Use this for initialization
	void Start () {
//        if (controls != null)
//            SceneManager.LoadScene(controls, LoadSceneMode.Additive);
        PlayerPrefs.SetInt("Editor Started", 1);
        if (environment != null)
            SceneManager.LoadScene(environment, LoadSceneMode.Additive);
        
        }
	
	// Update is called once per frame
	void Update () {
	
	}
}
