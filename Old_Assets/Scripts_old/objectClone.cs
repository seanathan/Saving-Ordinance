using UnityEngine;
using System.Collections;

public class objectClone : MonoBehaviour {
    public GameObject Cloning;

	// Use this for initialization
	void Clone () {

        if (transform.childCount > 0 && Cloning != null)
        {
            Instantiate(Cloning, transform.position, transform.rotation, transform);
            
        }


    }

    // Update is called once per frame
    void OnValidate()
    {
        Clone();
	}
}
