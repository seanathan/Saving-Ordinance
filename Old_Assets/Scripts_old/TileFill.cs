using UnityEngine;
using System.Collections;

public class TileFill : MonoBehaviour {
    //public GameObject[] fill;
    //public int pickFill = 0;
    public string cellStatus = "";

    [System.Serializable]
    public struct tileFill
    {
        public string status;
        public GameObject cell;
    }

    public tileFill[] fillStyle;


    // Update is called once per frame
    void Update()
    {
        foreach (tileFill tile in fillStyle)
            tile.cell.SetActive(tile.status == cellStatus);
        
    }
}
