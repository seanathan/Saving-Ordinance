using UnityEngine;
using System.Collections;

public class mover : MonoBehaviour {
    public Vector2 textureMover = new Vector2(0f,0f);
	


    void FixedUpdate () {

        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", GetComponent<Renderer>().material.GetTextureOffset("_MainTex") + textureMover);
        GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", GetComponent<Renderer>().material.GetTextureOffset("_BumpMap") + textureMover);
	}
}
