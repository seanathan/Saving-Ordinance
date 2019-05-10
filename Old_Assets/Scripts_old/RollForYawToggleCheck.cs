using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RollForYawToggleCheck : MonoBehaviour {

    public Color off = new Color32(255, 255, 255, 178);
    public Color on = new Color32(0, 255, 0, 178);



    // Update is called once per frame
    void Update()
    {
        ColorBlock colorEdit = gameObject.GetComponent<Button>().colors;
        if (PlayerControls.GetActivePlayer() == null)
            return;


        if (PlayerControls.GetActivePlayer().rollForYaw)
        {
            colorEdit.normalColor = on;
            colorEdit.highlightedColor = on;
            gameObject.GetComponent<Button>().colors = colorEdit;


        }
        else
        {
            colorEdit.normalColor = off;
            colorEdit.highlightedColor = off;
            gameObject.GetComponent<Button>().colors = colorEdit;
        }
    }
}
