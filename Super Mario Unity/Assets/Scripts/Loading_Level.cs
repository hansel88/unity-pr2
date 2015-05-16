using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading_Level : MonoBehaviour {
	public Text livesText;
	public string levelToLoad = "Level_01";
    float timer;
    float maxDelay;

	void Start () {
        timer = 0;
        maxDelay = 2;
		if (livesText)
			livesText.text = " x  " + Utils.LoadLives ();
	}

	void Update()
	{
        if (timer < maxDelay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Application.LoadLevel(levelToLoad);
        }
               
    }
}
