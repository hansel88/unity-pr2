using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
	public Text textScore;
	public Text textCoin;
	public Text textTime;
	public Text textLevel;

	public static GUIManager instance;

	void Awake()
	{
		instance = this;
	}

	public void ChangeScoreText(int score)
	{
		textScore.text = string.Format ("{0}", score);
	}

	public void ChangeCoinText(int coins)
	{
		textCoin.text = string.Format ("{0}", coins);
	}

	public void ChangeTimeText(int time)
	{
		textTime.text = string.Format ("{0}", time);
	}

	public void ChangeLevelText(string level)
	{
		textLevel.text = string.Format ("{0}", level);
	}
}