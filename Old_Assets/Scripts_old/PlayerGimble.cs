using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGimble : MonoBehaviour {
	public float pitch = 10.0f;
	public float yaw = 10.0f;
	public float tilt = 10.0f;
	public GameObject player;
	public Vector3 anglevel;

	public Text angularVelocityRead;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
	//	GetComponent<Rigidbody>().rotation = Quaternion.Euler (player.GetComponent<Rigidbody>().velocity.y * pitch, player.GetComponent<Rigidbody>().velocity.x * yaw, player.GetComponent<Rigidbody>().velocity.x * tilt);
	//	transform.localRotation = Quaternion.Euler (player.transform.localRotation.y * pitch, player.transform.localRotation.x * yaw, player.transform.localRotation.x * tilt);
	//	anglevel = new Vector3(player.GetComponent<PlayerControllerAlpha.>().angularVelocity.x, player.transform., player.GetComponent<Rigidbody>().angularVelocity.z);
		                                 

//		moveHorizontal = Input.GetAxis ("Horizontal") + rightButton - leftButton;
//		moveVertical =  Input.GetAxis ("Vertical") + upButton - downButton;


	//	anglevel.ToString(angularVelocityRead.text);
		//sense change in velocity

	}
}
