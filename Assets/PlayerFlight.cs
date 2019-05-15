using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlight : MonoBehaviour
{
    public SpaceShip playerShip;
    public GameObject navPointer;
    public float mh, mv, mt;
    public float scale = 1f;
    public Space space;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (navPointer) navPointer.transform.SetParent(playerShip?.Gyro.transform);

        mh = Input.GetAxis("Horizontal");
        mv = Input.GetAxis("Vertical");
        mt = Input.GetAxis("Jump");
        
        mh *= scale;
        mv *= scale;

        if (playerShip){
            //transform.Rotate(Vector3.up, mh, space);
            playerShip.yoke(mv,mh,0f);

            playerShip.throttle = mt;


        }
    }
}
