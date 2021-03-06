﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Handling the highscore text
public class MainMenuScript : MonoBehaviour {

    public Text scoreText;
	// Use this for initialization
	void Start () {
        string highscore = PlayerPrefs.GetInt("Highscore") + "";
        string prefix = "TOP- ";
        for (int i = 0; i < (8 - highscore.Length); i++)
        {
            prefix += "0";
        }

        scoreText.text = prefix + highscore;

		Utils.SaveLives (3); // Default to 3 lives
	}
}
