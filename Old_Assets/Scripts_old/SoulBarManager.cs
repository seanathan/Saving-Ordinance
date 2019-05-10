using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoulBarManager : MonoBehaviour {
    public TileFill[] soulElements;
    public GameObject tileToMake;
    public int soulsPerTile = 1000;

    public float livesMax;
    public float livesRrell;
    public float livesNative;
    public float livesSpared;

    public int tileCount = 10;
    public bool resize = false;

    void OnValidate()
    {
        if (resize)
        {
            AdjustTileCount();
            resize = false;
        }
        
    }

    void AdjustTileCount()
    {
        soulElements = transform.GetComponentsInChildren<TileFill>(false);  // init list 

        TileFill[] totalSoulElements = transform.GetComponentsInChildren<TileFill>(true);

        //transform.GetComponentsInChildren<TileFill>(false);

        //tilecount + 1 to skip prefab stamp at pos -1

        if (totalSoulElements.Length <= tileCount)                           // add extra elements
        {
            for (int i = totalSoulElements.Length; i <= tileCount; i++)
            {
                float gap = tileToMake.GetComponent<RectTransform>().rect.width;
                Vector3 spawnPoint = tileToMake.GetComponent<RectTransform>().localPosition + new Vector3(i* gap,0f,0f);
                GameObject newTile = (GameObject)(Instantiate(tileToMake, tileToMake.transform.position, tileToMake.transform.rotation, transform));
                newTile.GetComponent<RectTransform>().localPosition = spawnPoint;

            }
        }

        totalSoulElements = transform.GetComponentsInChildren<TileFill>(true);


        for (int i = 1; i < totalSoulElements.Length; i++)                  // activate all below tilecount
        {
            //if (i - 1 >= tileCount)
            //    Destroy(totalSoulElements[i].gameObject);
            
            if (i - 1 < tileCount)
                totalSoulElements[i].gameObject.SetActive(true);
            else
                totalSoulElements[i].gameObject.SetActive(false);
            
        }

        soulElements = transform.GetComponentsInChildren<TileFill>(false);  // refresh

    }

    void Start()
    {
        AdjustTileCount();
    }

    void Update()
    {
        if (livesMax < ScoreKeeper.soulsMax)
        {
            tileCount = ScoreKeeper.soulsMax / soulsPerTile;
            AdjustTileCount();
        }


        for (int i = 0; i < tileCount; i++)
        {

            livesMax = ScoreKeeper.soulsMax;
            livesRrell = ScoreKeeper.soulsRrell;
            livesNative = ScoreKeeper.soulsNative;
            livesSpared = ScoreKeeper.soulsSpared;

            // deadfill = 0
            // greenfill = 1
            // neutrfill = 2
            // redfill = 3



            //rell third
            if (i < Mathf.RoundToInt(tileCount * livesRrell / livesMax))
                soulElements[i].cellStatus = "Red";

            //spared second
            else if (i < Mathf.RoundToInt(tileCount * (livesRrell + livesSpared) / livesMax))
                soulElements[i].cellStatus = "Neutral";

            //alive first
            else if (i < Mathf.RoundToInt(tileCount * (livesRrell + livesSpared + livesNative) / livesMax))
                soulElements[i].cellStatus = "Green";

            //dead last
            else
                soulElements[i].cellStatus = "Dead";


            int dead = ScoreKeeper.soulsMax - ScoreKeeper.soulsRrell - ScoreKeeper.soulsNative;
        //    rrellMark.value = livesRrell / livesMax;
        //    nativeMark.value = (livesRrell + livesSpared + livesNative) / livesMax;
        //    sparedMark.value = (livesRrell + livesSpared) / livesMax;
        }
        //soulText.text = "Rrell Lives: " + livesRrell.ToString() + "\t\tLives Spared: " + livesSpared.ToString() + "\t\tDead: " + dead.ToString();

    }
}

