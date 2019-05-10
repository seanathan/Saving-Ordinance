using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class graphicPower : MonoBehaviour {
    private int qLevel;
    public int defaultQualValue = 0;
    public bool forceQuality = false;

    
        
    void Update()
    {
        qLevel = UserInfo.getQualityLevel();
        
        transform.GetComponent<Slider>().value = qLevel;
        
    }
    


    public void QualChange (float s)
    {
        UserInfo.SetQuality(s);
    }
}
