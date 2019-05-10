using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetList : EnemyShipModular {

    public GameObject[] targets;
    private int count;
    
	void Awake () {
        targets = null;
	}
	
    public void addTarget(GameObject mob, bool add = true)
    {
        GameObject[] oldList = targets;

        //check if target is in list
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == mob)
            {
                if (add)
                    return;
                else
                {
                    //remove association from list
                    targets[i] = null;
                }
            }

            //do not keep empty slots
            if (targets[i] == null)
                count--;
        }
        if (add)
            count++;

        //error catch
        if (count < 0)
        {
            count = 0;
            Debug.Log("Too many badguys removed");
        }

        //new list with no blank spots
        
        targets = new GameObject[count];

        int j = 0;
        int k = 0;
        while (j < count && k < oldList.Length)
        {

            //do not keep if blank

            if (oldList[k] == null)
                k++;
            else
            {
                targets[j] = oldList[k];
                j++;
                k++;
            }
        }
    }
    

    public void UpdateList()
    {
        
    }


    public GameObject getTarget()
    {
        return null;
    }


    public GameObject nextTarget()
    {
        return null;
    }
    
}
