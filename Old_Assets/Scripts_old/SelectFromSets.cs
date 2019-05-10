using UnityEngine;
using System.Collections;

public class SelectFromSets : MonoBehaviour {
    [Range(0,10)]
    public int activeSet = 0;
    
    public string currentSet;
    public SetOfGameObjects[] Sets;

    [Header("Debug")]
    public bool allActive = false;

    void OnValidate()
    {
        SetSelect(activeSet);
    }

    public SetOfGameObjects getActiveSet()
    {
        return Sets[activeSet];
    }

    public bool SetSelect(string alias)
    {
        
        for(int i = 0; i <Sets.Length; i++)
        {
            if (Sets[i].getAlias() == alias)
            {
                SetSelect(i);
                return true;
            }
        }

        //if failure

        SetSelect(0);
        return false;
    }

    public void SetSelect(float slideValue)
    {
        if (Sets.Length == 0)
        {
            Debug.Log("No options in" + gameObject.name);
            return;
        }
        
        slideValue = Mathf.Clamp(slideValue, 0, Sets.Length - 1);
                    
        activeSet = Mathf.RoundToInt(slideValue);

        //deactivate all first to avoid conflict
        for (int i = 0; i < Sets.Length; i++)
        {
            if (Sets[i] != null)
                Sets[i].Activate(allActive); //off if not debugging
        }

        //activate the correct one
        if (Sets[activeSet] != null)
        {

            Sets[activeSet].Activate(true);
            currentSet = Sets[activeSet].name;
        }
      //  PlayerPrefs.SetInt("LastUiType", uiType);
    }

    public void nextSet()
    {
        activeSet++;
        if (activeSet >= Sets.Length)
            activeSet = 0;

        SetSelect(activeSet);
    }

    public void prevSet()
    {
        activeSet--;
        if (activeSet < 0)
            activeSet = Sets.Length - 1;

        SetSelect(activeSet);
    }
}
