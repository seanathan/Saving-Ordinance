using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelTester : MonoBehaviour {
    public bool call = true;
    public int mapControls;

    //start at main menu;
	void Start () {
        
        SceneManager.LoadScene(00);

	}
	
}
