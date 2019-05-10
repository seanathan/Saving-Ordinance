using UnityEngine;
using System.Collections;
using System;

public class SetOfGameObjects : MonoBehaviour {

    public string alias;

    [System.Serializable]
    public struct setMember
    {
        public GameObject member;
        public bool willEnable;
    }

    public setMember[] setOfObjects;

    public enum Status
    {
        Waiting,
        Inactive,
        Active,
        ForceActive,
        ForceSuppressed
    }

    public Status status;
    
    
    void OnValidate()
    {
        if (status == Status.ForceActive)
        {
            Activate(true);
        }

        if (status == Status.ForceSuppressed)
        {
            Activate(false);
        }
    }

    public int Length()
    {
        return setOfObjects.Length;

    }

    public bool IsActivated()
    {
        return (status == Status.ForceActive || status == Status.Active);

        /*
        //check if any
        for (int i = 0; i < setOfObjects.Length; i++)
        {
            if (setOfObjects[i].willEnable && setOfObjects[i].member.activeInHierarchy)
                return true;

        }
        return false;*/
    }

    public void Activate(bool enabler)
    {
        if (status == Status.ForceActive)
            enabler = true;
        else if (status == Status.ForceSuppressed)
            enabler = false;
        else if (enabler)
            status = Status.Active;
        else
            status = Status.Inactive;
        
        //if nothing, enable root
        if (setOfObjects.Length == 0)
        {
            gameObject.SetActive(enabler);
        }

        foreach (setMember s in setOfObjects)
        {
            if (s.willEnable)
                s.member.SetActive(enabler);
        }

    }


    public string getAlias()
    {
        if (alias.Length > 0)
            return alias;
        else
            return (gameObject.name);
    }

}
