using UnityEngine;
using System.Collections;

public class ObjectEnableOnQuality : MonoBehaviour {
    
    public int currentQuality = 0;
	private bool _loaded = false;

    [System.Serializable]
    public struct QualItem
    {
        public string optionName;
        public GameObject qualItem;
        public bool[] setting;
    }

    public QualItem[] options;

    public void QualCheck()
    {
        if (currentQuality != UserInfo.getQualityLevel())
        {
            QualityChange(UserInfo.getQualityLevel());
        }
    }

    void OnValidate()
    {
		QualCheck();
        
    }
    

    void QualityChange(float q)
    {
		if (options == null)
			return;

        currentQuality = (int)q;

        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].setting.Length == 0)
                options[i].setting = new bool[3];

            if (options[i].qualItem != null)
            {
                options[i].optionName = options[i].qualItem.name;
                
                if (currentQuality < options[i].setting.Length - 1)
                    options[i].qualItem.SetActive(options[i].setting[currentQuality]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        QualCheck();

    }     
}
