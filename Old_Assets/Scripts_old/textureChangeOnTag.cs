using UnityEngine;
using System.Collections;

public class textureChangeOnTag : MonoBehaviour {

    public Material threatTexture;
    public Material allyTexture;
    private EnemyShipModular ship;

    void Start()
    {
        ship = transform.GetComponentInParent<EnemyShipModular>();
        TextureChange(ship);
    }


    public void TextureChange(EnemyShipModular selfShip)
    {    
        
        if (selfShip.gameObject.tag == "Threat" || selfShip.gameObject.tag == "Boss")
        {
            gameObject.GetComponent<Renderer>().material = threatTexture;
        }
        else if   (selfShip.gameObject.tag == "Ally" || selfShip.gameObject.tag == "Objective")
        {
            gameObject.GetComponent<Renderer>().material = allyTexture;
        }
    }
}
