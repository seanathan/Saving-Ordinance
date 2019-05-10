using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivePopUpText : MonoBehaviour {
    public GameObject activeTarget;
    private Vector3 restPosition;
    private Quaternion restRotation;
    public Text output;
    public static string message;
    public DialogueBox masterText;
    private float resetTimer = 2.0f;
    private float resetcountdown = 0.0f;
    private string last;

	
    void Start()
    {
        restPosition = output.rectTransform.localPosition;
        restRotation = transform.localRotation;
    }

	// Update is called once per frame
	void Update () {
        if (DialogueBox.tracking != null)
        {
            activeTarget = DialogueBox.tracking;
        }

        if (activeTarget == null)
        {
            transform.localRotation = restRotation;
            output.text = " ";

        }
        else
        {
            output.text = message;
            float bright = resetcountdown / resetTimer;

            output.color = new Color(output.color.r, output.color.g, output.color.b, bright);

        }

          if (message != last)
            {
                resetcountdown = resetTimer;
            }

            if (resetcountdown < 0.5f)
            {
            resetcountdown = 0.0f;
            }
            if (resetcountdown > 0.0f)
            {
                resetcountdown = resetcountdown - Time.deltaTime;
            }
         last = message;
	}   


    public void TrackOnHUD()
    {
        if (activeTarget != null)
        {
            transform.LookAt(activeTarget.transform);
        }
    }
}
