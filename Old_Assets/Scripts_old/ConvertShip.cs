using UnityEngine;
using System.Collections;

public class ConvertShip : MonoBehaviour {


    public bool converting;
    public GameObject target;
    public GameObject flood;

    public float attackRange = 300.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (converting) // && attackRange > Vector3.Distance(transform.position, target.transform.position))
        {
            transform.LookAt(target.transform.position);
            Instantiate(flood, transform.position, transform.rotation);
        }
	}
}
