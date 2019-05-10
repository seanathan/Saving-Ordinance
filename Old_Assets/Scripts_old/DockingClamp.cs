using UnityEngine;
using System.Collections;

public class DockingClamp : MonoBehaviour {
    public GameObject DockingGround;
    public GameObject DockedShip;
    public float tetherLength = 0f; //0 for no tether
   
	
	// Update is called once per frame
	void Update () {

        if (DockingGround != null && tetherLength > 0)
        {
            
            //find docking ground
            transform.position = DockingGround.transform.position;
            transform.rotation = DockingGround.transform.rotation;

            float tether = Vector3.Distance(transform.position, DockedShip.transform.position);

            if (tether > tetherLength)
            {
                //release
                EngageClamp();
                transform.localPosition = Vector3.zero;
            }

            //engage if dock set but not parented.
            if (transform.IsChildOf(DockedShip.transform))
                EngageClamp(DockedShip);
        }

        

        //match transformation

        //parent the docked ship to the docking ground


    }

    //mocks parenting to dock without changing
    public void EngageClamp(GameObject LandingSite = null)
    {
        //releases if called null

        DockingGround = LandingSite;


        if (LandingSite != null)
        {
            gameObject.name = DockedShip.name + " [DOCKED]";
            transform.SetParent(null);

            transform.position = DockingGround.transform.position;
            transform.rotation = DockingGround.transform.rotation;

            DockedShip.transform.SetParent(transform);
        }

        else
        {

            gameObject.name = "Docking Clamp";
            DockedShip.transform.SetParent(null);

            //transform.localPosition = Vector3.zero;
            transform.SetParent(DockedShip.transform);

        }
    }



}
