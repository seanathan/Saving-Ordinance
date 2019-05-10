using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif


public class EditorControls : MonoBehaviour {
    public string controls;
    public bool LoadPlayerControls = false;
    public bool popUpRequest = true;


    public enum con
    {
        waiting,
        notLoaded,
        loading,
        loaded
    }
    public con sk;

#if UNITY_EDITOR
    
    void OnValidate ()
    {

        //  if (EditorSceneManager.loadedSceneCount < 2 && EditorSceneManager.loadedSceneCount > 0 )
        //enabler = true;

        /*
        if (EditorSceneManager.loadedSceneCount > 0 && popUpRequest)
        {
            if (EditorSceneManager.loadedSceneCount < 2)
            {
                ControlOption();
            }
        }
        */

        if (sk == con.waiting) {
            //wait for this scene to finish loading.
            sk = con.notLoaded;
        }

        if (sk == con.notLoaded) {

        }

        if (sk == con.loading) {
            
            //wait for right time
        }

        if (sk == con.loaded)  {
            //all set!
        }



        if (LoadPlayerControls && EditorSceneManager.loadedSceneCount < 2)
        //if (EditorSceneManager.loadedSceneCount < 2 && EditorSceneManager.loadedSceneCount > 0)
        {
			// EditorSceneManager.OpenScene(controls, OpenSceneMode.Additive);
			LoadControls();
			//ControlOption();
			LoadPlayerControls = false;
        }
    }
    
    public void ControlOption()
    {
        if (EditorUtility.DisplayDialog("Initialize Game Controls",
            "Would you like to load the game controls?", "Yes", "No")) {
            //wait for level to finish loading
            LoadControls();
        }
        else {
            popUpRequest = false;
        }
    }

    public void LoadControls() {
        EditorSceneManager.OpenScene(controls, OpenSceneMode.Additive);
    }


#endif
}


