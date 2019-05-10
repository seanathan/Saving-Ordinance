using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyShieldScript : MonoBehaviour {

	public float shieldHP = 500.0f;
	public GameObject shieldText;

	// Use this for initialization
	void Start () {
		shieldText.GetComponent<TextMesh> ().text = shieldHP.ToString ();
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PlayerBullet") 
		{
			BulletScript1 incoming = other.gameObject.GetComponent<BulletScript1>();
			shieldHP = shieldHP - incoming.damage;
			Debug.Log("Impact");
			shieldText.GetComponent<TextMesh> ().text = shieldHP.ToString ();

		}
		
	}



	void LateUpdate()
	{
		if (shieldHP <= 0.0f) 
		{
			DialogueBox.Dialogue.text = "Target Shields Disabled";
			Destroy(gameObject);
		}
	}
}
