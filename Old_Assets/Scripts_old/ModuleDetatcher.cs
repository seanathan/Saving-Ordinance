using UnityEngine;
using System.Collections;

public class ModuleDetatcher : MonoBehaviour {
    public bool detached;
    public float modHP = 100.0f;
    public Transform explosionPoint;
    public float eF;
    public float eR;

    //public float modMaxHP = 100.0f;

    public bool flash = false;

    public bool drop = true;
    public GameObject winDrop;
   // private bool EngineStart = true;

    void Start()
    {
        Rigidbody modRB = transform.GetComponent<Rigidbody>();
        detached = false;
        //GetComponent<Renderer>().enabled = false;
        //modHP = modMaxHP;
        modRB.isKinematic = true;
       // modRB.constraints = RigidbodyConstraints.FreezeAll;
    }

//    public void RestoreEngines()
//    {
//        if (EngineStart)
//        {
//            modMaxHP = modHP;
//            EngineStart = false;
//        }   
//        enginePower = 1.0f;
//        if (engine1 != null)
//            engine1.SetActive(true);
//        if (engine2 != null)
//            engine2.SetActive(true);
//
//        modHP = modMaxHP;
    //    }

    public void BulletImpact(float damage)
    {
        modHP -= damage;
    }



    void LateUpdate()
    {
        if (modHP <= 0.0f || detached == true) 
        {
//            enginePower = 0;
//            if (engine1 != null)
//                engine1.SetActive(false);
//            if (engine2 != null)
//                engine2.SetActive(false);
//
            detached = true;

            Rigidbody modRB = transform.GetComponent<Rigidbody>();
            if (modRB != null)
            {
                modRB.isKinematic = false;
                modRB.constraints = RigidbodyConstraints.None;
            //    Vector3 eDirection = explosionPoint.transform.position - transform.root.transform.position;
                modRB.AddExplosionForce(eF, explosionPoint.position, eR);
                //modRB.AddForceAtPosition(eF*eDirection.normalized, explosionPoint.position);
                //modRB.angularVelocity = GetComponentInParent<Rigidbody>().angularVelocity;
                //modRB.velocity = GetComponentInParent<Rigidbody>().velocity;
            }

            transform.SetParent(null, true);
            //transform.rotation = transform.root.transform.rotation;
                transform.GetComponent<ModuleDetatcher>().enabled = false;
        }
    }
}
