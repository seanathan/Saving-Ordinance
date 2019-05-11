using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazyrotate : MonoBehaviour
{
    [Range(-5,5)]
    public int rotSpeed = 1;
    public float bankMax = 90;

    public bool banked = false;
    public bool banking = false;

    public GameObject shipModel;
    public GameObject refPlate;

    // Update is called once per frame
    void Update()
    {
        float y = shipModel.transform.rotation.eulerAngles.y;
        float z = shipModel.transform.rotation.eulerAngles.z;
        if (!banking){
            y = Mathf.MoveTowards(y, 360, rotSpeed);
            if ( y >= 360.0f - rotSpeed) banking = true;
        }
        else {
            float zb = (banked)?0f:bankMax;
            
            z = Mathf.MoveTowardsAngle(z, zb, rotSpeed);

            if ( Mathf.Abs( z-zb )< rotSpeed) {
                banking = false;
                banked = !banked;
            }
        }
        Quaternion qs = Quaternion.Euler(0,y,z);
        if (shipModel) shipModel.transform.SetPositionAndRotation(shipModel.transform.position, qs);

        Quaternion qr = Quaternion.Euler(0,y,z);
        if (refPlate) refPlate.transform.SetPositionAndRotation(refPlate.transform.position, qr);
    }
    
}
