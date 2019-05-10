using UnityEngine;
using System.Collections;
// using UnityStandardAssets.Cameras;

public class ViewYourTarget : MonoBehaviour {
    private GameObject gyro;
    public bool testCamFit = false;
    public float followSpeed = 1;
    // Use this for initialization
    void Start()
    {
        transform.GetComponent<Camera>().enabled = false;
        //    transform.GetComponent<LookatTarget>().enabled = false;
        //   transform.GetComponent<TargetFieldOfView>().enabled = false;


        //free from hierarchy

        transform.SetParent(null);


        gyro = new GameObject("navGyro Null");
        gyro.transform.position = transform.position;
        gyro.transform.SetParent(transform);
    }

    void OnValidate()
    {
        if (testCamFit)
            GetComponent<Camera>().rect.Set(0f, 0f, 1f, 1f);
        testCamFit = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Camera>().rect.Set(0f, 0f, 1f, 1f);
        transform.position = Vector3.Lerp(transform.position, ScoreKeeper.playerAlive.transform.position, Time.deltaTime * followSpeed);

        if (DialogueBox.tracking == null)
        {
            transform.GetComponent<Camera>().enabled = false;
      //      transform.GetComponent<LookatTarget>().enabled = false;
      //      transform.GetComponent<TargetFieldOfView>().enabled = false;
        }
        else if (DialogueBox.tracking != null)
        {


            transform.GetComponent<Camera>().enabled = true;
            gyro.transform.LookAt(DialogueBox.tracking.transform);

            transform.rotation = Quaternion.Lerp(transform.rotation, gyro.transform.rotation, Time.deltaTime * followSpeed);

      //      transform.GetComponent<LookatTarget>().enabled = true;
      //      transform.GetComponent<TargetFieldOfView>().enabled = true;
      //      transform.GetComponent<LookatTarget>().SetTarget(DialogueBox.tracking.transform);
      //      transform.GetComponent<TargetFieldOfView>().SetTarget(DialogueBox.tracking.transform);
        }
    }
}
