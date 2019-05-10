using UnityEngine;
using System.Collections;

public class MoveWithPlayerShip : MonoBehaviour {
    public Vector3 offset;

	// Update is called once per frame
	void LateUpdate () {
        Transform player = ScoreKeeper.playerAlive.GetComponent<PlayerControllerAlpha>().activeChassis.transform;
       // if (offset == Vector3.zero)
       //     offset = transform.position - player.position;

        transform.position = player.position + offset;
        transform.rotation = player.rotation;

        // ScoreKeeper



        //disable with player
        if (PlayerControls.getPlayerHP() < 1.0f)
            gameObject.SetActive(false);

    }
}
