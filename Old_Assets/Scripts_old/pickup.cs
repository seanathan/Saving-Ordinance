using UnityEngine;
using System.Collections;

public class pickup : MonoBehaviour {

    public bool cap = false;
    public bool HP = false;

    public float value = 50.0f;

    public bool disable = true;

	// Update is called once per frame
	void LateUpdate ()
    {
        if (DropCleanup.dropcleaner != null)
            transform.SetParent(DropCleanup.dropcleaner.transform);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (HP)
            {
                PlayerControls.getPlayerShip().modHP(value);
            }

            if (cap)
            {
                PlayerControls.GetActivePlayer().charge += value;
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        gameObject.SetActive(!disable);
    }
}
