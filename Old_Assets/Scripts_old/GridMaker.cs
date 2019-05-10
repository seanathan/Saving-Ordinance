using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridMaker : MonoBehaviour {

    public Image gridBar;
    public int gridBarCount = 50;
    public int spacing = 20;
    public bool fillGrid = false;




    void Awake()
    {
        SetFloor();

    }

    void Update()
    {
        if (fillGrid)
        {
            SetFloor();
            fillGrid = false;

        }
    }


    // Use this for initialization
    void SetFloor()
    {

        for (int i = 0; i < gridBarCount; i++)
        {
            Instantiate(gridBar, new Vector3(i * spacing, 0f, 0f), gridBar.transform.rotation);

        }
    }
}
