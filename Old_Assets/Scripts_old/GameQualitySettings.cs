using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQualitySettings : MonoBehaviour {

    public enum gameQuality
    {
        low = 0,
        medium = 1,
        high = 2
    }
    
    public static int level = 0;
    
    //THIS IS THE MASTER SWITCH
    public gameQuality masterQuality;

    public bool MasterSwitch = true;

    public gameQuality qualityListener;

    public static gameQuality Qual(float i)
    {
        i = Mathf.Clamp(i, 0, 2);

        return ((gameQuality)i);
    }
    
    
    public void QualCheck()
    {
        if (MasterSwitch)
        {
            QualityChange(masterQuality);
        }
        
        qualityListener = Qual(QualitySettings.GetQualityLevel());

    }

    public void QualityChange(float q)
    {
        QualityChange(Qual(q));
    }

    public void QualityChange(gameQuality q)
    {
        masterQuality = q;
        level = (int)masterQuality;

        if (level == QualitySettings.GetQualityLevel())
            return;

        QualitySettings.SetQualityLevel(level);
        Graphics.activeTier = (UnityEngine.Rendering.GraphicsTier)level;
       
        PlayerPrefs.SetInt("Game Graphics", level);


        Debug.Log("Quality Changed to " + Qual(level));

    }

    void Awake()
    {
      //  level = PlayerPrefs.GetInt("Game Graphics");

        QualCheck();
    }

    void OnValidate()
    {
      //  level = PlayerPrefs.GetInt("Game Graphics");

        QualCheck();
        
        /*
        for (int i = 0; i < allObjects.Length; i++)
        {
            CamQualChanger cam = allObjects[i].GetComponent<CamQualChanger>();
            ObjectEnableOnQuality obj = allObjects[i].GetComponent<ObjectEnableOnQuality>();

            if (cam)
                cam.QualCheck();
            if (obj)
                obj.QualCheck();


        }
        */
        

    }

    // Update is called once per frame
    void Update () {
        QualCheck();


    }
}
