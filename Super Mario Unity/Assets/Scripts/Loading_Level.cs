using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Loads a new level after x seconds
public class Loading_Level : MonoBehaviour {
	public Text livesText;
	public string levelToLoad = "Level_01";
    public float maxDelay = 2f;
	private float timer;

	void Start () {
        timer = 0;

		// Set the amount of lives text
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
