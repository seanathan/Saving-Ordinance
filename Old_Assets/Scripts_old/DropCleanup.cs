using UnityEngine;
using System.Collections;

public class DropCleanup : MonoBehaviour {


    public static DropCleanup dropcleaner;

    void Start()
    {
        if (dropcleaner == null)
        {

            dropcleaner = this;
        }
        if (dropcleaner != this)
        {
            Destroy(gameObject);
        }

    }

 
}
