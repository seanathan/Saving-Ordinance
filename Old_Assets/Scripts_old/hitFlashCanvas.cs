using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hitFlashCanvas : MonoBehaviour {
    [Header("Flashes Red on Player HP Deduction")]
    private Image UIelement;
    private Color normal;
    private Color redFlash = new Color(1,0,0,1);
    public float playerHPLast;
    public bool camNoise = false;

    // Use this for initialization
	void Start () {
        UIelement = transform.GetComponentInChildren<Image>();
        normal = UIelement.color;
        //playerHPLast = PlayerControllerAlpha.getPlayerHP();
        
        playerHPLast = PlayerControls.getPlayerShip().hullHP;
    }
	
    public void HitFlash (Color color) 
    {
        UIelement.color = color;
	}


    void Update()
    {
        redFlash = ScoreKeeper.hitFlashColor;
        UIelement.color = Color.Lerp(UIelement.color, normal, Time.deltaTime * ScoreKeeper.hitFlashRefreshRate);
        if (playerHPLast > PlayerControls.getPlayerHP())
            HitFlash(redFlash);
        
        playerHPLast = PlayerControls.getPlayerHP();
        if (PlayerControls.GetActivePlayer().crashCountdown > 0.15f)
        {
            if (GetInstanceID() * 2  * (1.5f - (PlayerControls.GetActivePlayer().crashCountdown / PlayerControls.GetActivePlayer().crashTimer)) * Random.value > GetInstanceID())
                HitFlash(new Color(0,0,0,0));
        }
     
//        NoiseAndScratches grainOut = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NoiseAndScratches>();
  //      grainOut.enabled = true;
    //    grainOut.grainIntensityMax = Mathf.Clamp(5.0f, 0.0f, 5.0f);
      //  grainOut.grainIntensityMin = Mathf.Clamp(4.0f, 0.0f, 5.0f);

   
    //        float playerIntegrityRatio = PlayerControllerAlpha.playerHP / playerCon.activeChassis.hullHP;
    //        if (playerIntegrityRatio < 0.3f)
    //        {
    //            if (GetInstanceID() * 20 * playerIntegrityRatio * Random.value < GetInstanceID())
    //                HitFlash(new Color(0,0,0,0));
    //        }
}
}
