using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShieldArc : MonoBehaviour {
    public bool shields;
    public bool HP;
    public bool shieldsInLine;
    public bool Cap;
    public bool slide;
    public bool slider = false;
    public float value = 1.0f;
    public float maxSize;

    void Start()
    {
        maxSize = GetComponent<RectTransform>().rect.width;
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerControls.getPlayerShip() == null)
            return;

        //get the current Shield or HP, display
        if (slide)
        {
            if (shields)
                value = (PlayerControls.getPlayerShip().shieldHP / PlayerControls.getPlayerShip().shieldHPMax);
            if (HP)
                value = (PlayerControls.getPlayerShip().getHealth().ratio);
            if (shieldsInLine)
                value = ((PlayerControls.getPlayerShip().shieldHP + PlayerControls.getPlayerShip().getHealth().currentHP) / PlayerControls.getPlayerShip().getHealth().maxHP);
            if (Cap)
                value = (PlayerControls.GetActivePlayer().charge / PlayerControls.GetActivePlayer().chargeMax);

            if (slider)
                GetComponent<Slider>().value = value;

            else
                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value * maxSize);


        }

        else
        {
            if (shields)
                GetComponent<Image>().fillAmount = (PlayerControls.getPlayerShip().shieldHP / PlayerControls.getPlayerShip().shieldHPMax);
            if (HP)
                GetComponent<Image>().fillAmount = (PlayerControls.getPlayerShip().getHealth().ratio);
            if (shieldsInLine)
                GetComponent<Image>().fillAmount = ((PlayerControls.getPlayerShip().shieldHP + PlayerControls.getPlayerShip().getHealth().currentHP) / PlayerControls.getPlayerShip().getHealth().maxHP);
            if (Cap)
                GetComponent<Image>().fillAmount = (PlayerControls.GetActivePlayer().charge / PlayerControls.GetActivePlayer().chargeMax);
        }
    }
}
