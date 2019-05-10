using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class shipMap : MonoBehaviour {
    public GameObject basicShip;
    public GameObject[] threats;
    public GameObject[] threatIcons;
    public bool run = false;
    public float scalar = 100f;


	// Update is called once per frame
	void Update () {
        if (run)
        {
            SpawnIcons("Threat");
            SpawnIcons("Player");
            run = false;
        }
	}

    void SpawnIcons(string tagthing)
    {
        threats = GameObject.FindGameObjectsWithTag(tagthing);
        for (int i = 0; i < threats.Length; i++)
        {
            GameObject icon = (GameObject)(Instantiate(basicShip, transform));
            shipMapElement sme =  icon.GetComponent<shipMapElement>();
            sme.sizeScalar = scalar;
            sme.myShip = threats[i];
        }
        
    }
}
