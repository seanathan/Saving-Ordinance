using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour {

    public string alias;

	

    public string getAlias()
    {
        if (alias.Length > 0)
            return alias;
        else
            return (gameObject.name);
    }

}
