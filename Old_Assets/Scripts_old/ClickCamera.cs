using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickCamera : MonoBehaviour {
	private bool freeLook;
	public bool rFollow = false;
	public bool eFollow = false; 

	public GameObject target;
	public GameObject player;
	public HUDTracker hud;
	public Raimi raimi;

	public Text mText;

	public float lookdead = 0.2f;

	public float contF = 2.0f;
	private float mouseX = 0.0f;
	private float mouseY = 0.0f;
	public float mouseF = 0.2f;
	public Vector3 scalar;
	public Vector3 camSize;
	public float wheelPos = 0.0f;
	public float minVel = 20.0f;

	public float lookTimeMax = 2.0f;
	private float lookTimer;
	public float lerpTimer = 0.0f;
	public float lerpTime = 1.0f;
	private Quaternion lastlook;


	void Start()
	{
		freeLook = false;
		target = player;
		}

	void FixedUpdate()
		{
			

		//target selection to follow

		if (rFollow == true) 
		{
			target = raimi.gameObject;
			eFollow = false;
		}
		else if (eFollow == true) 
		{
			target = hud.activeTarget;
			rFollow = false;
		}
		else {target = player;}



			float mouseW = Input.mouseScrollDelta.y;
		wheelPos = wheelPos - mouseW;

		//max wheel
		if (wheelPos > 9) 
		{
			wheelPos = 9;
				}
		//min wheel
		if (wheelPos < -9) 
		{
			wheelPos = -9;
		}

		//zoom alter
		camSize = scalar * (1 + (0.1f * wheelPos));


		transform.localScale = camSize;
		}

	void LateUpdate () 
	{
//UNUSED		Vector3 mousePo = Input.mousePosition;

		//freelook engage

		transform.position = target.transform.position;



		if (Input.GetButton ("Fire2") || Mathf.Abs (Input.GetAxis("VerticalLook")) >lookdead ||  Mathf.Abs (Input.GetAxis("HorizontalLook")) >lookdead) 
				{
			freeLook = true;
			lookTimer = lookTimeMax;


				}

		else if (target.GetComponent<Rigidbody>().velocity.magnitude < minVel && freeLook != true) 
		         {
			mouseX = 0.0f; 
			mouseY = 0.0f;
		}

		//freelook release

		else if (lookTimer > 0.1)
		{
			lookTimer = lookTimer - Time.deltaTime; 
			mouseX = 0.0f; 
			mouseY = 0.0f;
			freeLook = false;
			lerpTimer = 0; 
			lastlook = transform.rotation;}


		//lerpback


		if (lookTimer < 0.1 && lerpTimer < 1 && freeLook == false) {

			Vector3 pMomentum = target.GetComponent<Rigidbody>().velocity.normalized;
			Quaternion chase = Quaternion.LookRotation (pMomentum);
			transform.rotation = Quaternion.Lerp(lastlook, chase, lerpTimer);
			lerpTimer = lerpTimer + (Time.deltaTime / lerpTime);
		}


		//momentum-based camera
		if (freeLook == false && lerpTimer > 0.95) 
			{
				Vector3 pMomentum = target.GetComponent<Rigidbody>().velocity.normalized;
				Quaternion chase = Quaternion.LookRotation (pMomentum);
				transform.rotation = chase;
			}

		//freelooking on mouse

		if (freeLook == true)
			{
//				float xx = Input.mousePosition;
//				float yy = 0.0f;
//				float zz = 0.0f;
//			mouseX = mouseX + (mouseF * Input.GetAxis ("Mouse X"));
//			mouseY = mouseY + (mouseF * Input.GetAxis ("Mouse Y"));
//
			mouseX =  (mouseF * Input.GetAxis ("Mouse X")) + (contF * Input.GetAxis("HorizontalLook") * Mathf.Abs ( Input.GetAxis("HorizontalLook")));
			mouseY =  (mouseF * Input.GetAxis ("Mouse Y")) + (contF * Input.GetAxis("VerticalLook") * Mathf.Abs ( Input.GetAxis("VerticalLook")));
			transform.Rotate(-mouseY, mouseX, 0.0f);


			}

		mText.text = "MousePos = " + mouseX.ToString () + mouseY.ToString();	
	
	}
}
