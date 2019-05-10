using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour {
    
	public float livesMax;
	public float livesRrell;
	public float livesNative;
	public float livesSpared;
	public Slider rrellMark;
	public Slider nativeMark;
	public Slider sparedMark;
	public Text soulText;

    //deadmax for each star
    public int deadAllowed3Star = 10;
    public int deadAllowed2Star = 100;
    public int deadAllowed1Star = 1000;




    void Update()
    {
        livesMax = ScoreKeeper.soulsMax;
		livesRrell = ScoreKeeper.soulsRrell;
		livesNative = ScoreKeeper.soulsNative;
		livesSpared = ScoreKeeper.soulsSpared;
		int dead = ScoreKeeper.soulsMax - ScoreKeeper.soulsRrell - ScoreKeeper.soulsNative;
		rrellMark.value = livesRrell / livesMax;
		nativeMark.value = (livesRrell + livesSpared + livesNative) / livesMax;
		sparedMark.value = (livesRrell + livesSpared) / livesMax;

		soulText.text = "Rrell Lives: " + livesRrell.ToString () + "\t\tLives Spared: " + livesSpared.ToString() + "\t\tDead: " + dead.ToString ();
        
	}
}
