using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoulWrapup : MonoBehaviour {
    public float livesMax;
    public float deadTotal;
    public float livesSpared;
    
    public void EndSoulCount()
    {
        livesMax = ScoreKeeper.soulsMax;
        deadTotal = livesMax - ScoreKeeper.soulsRrell - ScoreKeeper.soulsNative - ScoreKeeper.soulsSpared;
        
        livesSpared = ScoreKeeper.soulsSpared;

        int dead = ScoreKeeper.soulsMax - ScoreKeeper.soulsRrell - ScoreKeeper.soulsNative;
        
        GetComponent<Text>().text = string.Format("<b>Loss of Life:</b>");
        GetComponent<Text>().text += string.Format("\n\t<b>{0,-6}</b> \tSouls at risk", livesMax);
        GetComponent<Text>().text += string.Format("\n\t<b>{0,-6}</b> \tLives Lost", deadTotal);
        GetComponent<Text>().text += string.Format("\n\t<b>{0,-6}</b> \tRrell Lives Spared", livesSpared);
    }
}