using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class extraLives : MonoBehaviour {
    public int lifeCount = 0;
    public int lifechanging = 0;
    public float spacing = 50f;

    

    [System.Serializable]
    public struct ships
    {
        public string shipName;
        public Image shipIcon;
        public int pos;
        public bool available;
    }

    public bool PullNewIcons = false;

    public ships[] lifeIcons;

	// Use this for initialization
	void LifeChange () {
        //if difference, 

        /*
        lifechanging = PlayerPrefs.GetInt(ScoreKeeper.mapNumber + " ShipsRemaining") - lifeCount;

        if (lifechanging == 0)
        {
            return;
        }

        if (lifechanging > 0)
        {
            //add one
            lifeCount++;
        }

        if (lifechanging < 0)
        {
            //drop one
            lifeCount--;
        }

        if (lifeCount > lifeIcons.Length)
        {
            //problem!  not enough icons!

            //fix later!

            return;
        }

        if (lifeCount < 0)
        {
            //ERROR
            return;
        }*/

        //lifeCount = PlayerPrefs.GetInt(ScoreKeeper.mapNumber + " ShipsRemaining");
        lifeCount = PlayerPrefs.GetInt("TotalShipsRemaining");

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            //if ACTIVE
           
            lifeIcons[i].available = (i < lifeCount);
            lifeIcons[i].shipIcon.gameObject.SetActive(i < lifeCount);
            lifeIcons[i].pos = i;
            lifeIcons[i].shipIcon.transform.localPosition =
                    new Vector2(spacing * lifeIcons[i].pos, 0);
        }
        /*
        if ((PlayerPrefs.GetInt(ScoreKeeper.mapNumber + " ShipsRemaining") - lifeCount) != 0)
        {
            LifeChange();
        }*/
        
    }
	
	// Update is called once per frame
	void Update () {
        //if difference, call a change
        LifeChange();


    }

    void OnValidate()
    {
        if (PullNewIcons)
        {
            Image[] newIcons = GetComponentsInChildren<Image>(true);

            lifeIcons = new ships[newIcons.Length];

            for (int i = 0; i < lifeIcons.Length; i++)
            {
                lifeIcons[i].shipIcon = newIcons[i];
                lifeIcons[i].shipName = "Basic #" + i;
            }

            PullNewIcons = false;
        }

        LifeChange();

        
    }
}
