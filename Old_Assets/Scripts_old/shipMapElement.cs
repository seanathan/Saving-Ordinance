using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class shipMapElement : MonoBehaviour {
    public GameObject myShip;
    public Image myImage;
    public float sizeScalar = 50f;
    public Color threatColor;
    public Color playerColor;

    void Start()
    {
        myImage = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update () {
        transform.localPosition = new Vector2(myShip.transform.position.x / sizeScalar, myShip.transform.position.y / sizeScalar);
        if (myShip.tag == "Threat")
            myImage.color = threatColor;
        if (myShip.tag == "Player")
            myImage.color = playerColor;


    }
}
