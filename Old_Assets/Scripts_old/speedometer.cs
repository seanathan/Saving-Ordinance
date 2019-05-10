using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class speedometer : MonoBehaviour {

    [Multiline]
    public string header = "Rel. Velocity: ";
    public bool _3DRelative = false;
    private Rigidbody playerrb;
    public float relForward;
    public float relUp;
    public float relRight;
    public bool marker = false;
    public Slider slForward;
    public Slider slRight;
    public Slider slUp;



    // Update is called once per frame
    void Update()
    {
        if (ScoreKeeper.scoreKeeperStarted == false)
            return;

        if (PlayerControls.getPlayerShip() == null)
            return;

        playerrb = PlayerControls.getPlayerShip().getRigidBody();

        Text my = GetComponent<Text>();

        if (!_3DRelative)
        {

            float speed = ScoreKeeper.playerSpeed;

            my.text = header + Mathf.RoundToInt(speed).ToString();
        }

        if (_3DRelative)
        {
            relForward = Vector3.Dot(playerrb.velocity, ScoreKeeper.playerAlive.transform.forward);
            relRight = Vector3.Dot(playerrb.velocity, ScoreKeeper.playerAlive.transform.right);
            relUp = Vector3.Dot(playerrb.velocity, ScoreKeeper.playerAlive.transform.up);


            my.text = string.Format(header, Mathf.Round(relForward), Mathf.Round(relRight), Mathf.Round(relUp));
        }
        //"Forward Velocity: {0}\tLat: {1} \tVert: {2}"

        //velocity marker?
        if (marker)
        {
            if (ScoreKeeper.playerSpeed > 0)
            {

                slForward.value = Mathf.Abs(relForward / ScoreKeeper.playerSpeed);
                slUp.value = Mathf.Abs(relUp / ScoreKeeper.playerSpeed);
                slRight.value = Mathf.Abs(relRight / ScoreKeeper.playerSpeed);
            }
            else
            {
                slForward.value = 0;
                slRight.value = 0;
                slUp.value = 0;
                
            }


        }
    }
}
