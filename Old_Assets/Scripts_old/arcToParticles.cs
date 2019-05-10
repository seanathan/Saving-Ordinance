using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class arcToParticles : MonoBehaviour {
    public float ratio;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ParticleSystem adjusted = GetComponent<ParticleSystem>();
        //var sh = adjusted.shape;
        ParticleSystem.ShapeModule sh = adjusted.shape;
        //sh.angle = 90.0f * GetComponentInParent<Image>().fillAmount;

        sh.angle = Mathf.PI * 0.5f * GetComponentInParent<Image>().fillAmount;

        ratio = GetComponentInParent<Image>().fillAmount;
        //ParticleSystemShapeType.CircleEdge = sh;



    }
}
