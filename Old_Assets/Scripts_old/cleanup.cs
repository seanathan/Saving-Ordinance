using UnityEngine;
using System.Collections;

public class cleanup : MonoBehaviour {
    public static cleanup cleaner;
    

	// Use this for initialization
	void Awake () {
        if (cleaner == null)
        {

            cleaner = this;
        }
        if (cleaner != this)
        {
            Destroy(gameObject);
        }

    }
    
    public static Transform getCleaner()
    {
        if (cleaner == null)
            cleaner = makeCleaner();

        return cleaner.transform;   
    }

    public static cleanup makeCleaner()
    {
        GameObject cgo = new GameObject("shotCleanup null");
        cleanup cnew = cgo.AddComponent<cleanup>();

        return cnew;
        
    }
    

}
