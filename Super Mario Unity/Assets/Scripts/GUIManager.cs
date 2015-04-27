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
		if (!textScore) 
		{
			Debug.LogError ("No textScore assigned!", this);
			return;
		}

		textScore.text = string.Format ("MARIO\n{0}", score);
	}

	public void ChangeCoinText(int coins)
	{
		if (!textCoin) 
		{
			Debug.LogError ("No textCoin assigned!", this);
			return;
		}

		textCoin.text = string.Format ("x{0}", coins);
	}

	public void ChangeTimeText(int time)
	{
		if (!textTime) 
		{
			Debug.LogError ("No textTime assigned!", this);
			return;
		}

		textTime.text = string.Format ("TIME\n{0}", time);
	}

	public void ChangeLevelText(string level)
	{
		if (!textLevel) 
		{
			Debug.LogError ("No textLevel assigned!", this);
			return;
		}

		textLevel.text = string.Format ("WORLD\n{0}", level);
	}
}