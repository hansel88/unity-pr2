using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// For handling GUI related tasks
public class GUIManager : MonoBehaviour
{
	public Transform rewardCanvasTransform; // Transform for the reward canvas
	public Text textScore;
	public Text textCoin;
	public Text textTime;
	public Text textLevel;
	public GameObject rewardPrefab;

	public static GUIManager instance;

	void Awake()
	{
		instance = this;
	}

	public void ChangeScoreText(int score)
	{
		// Make sure a text object is assigned
		if (!textScore) 
		{
			Debug.LogError ("No textScore assigned!", this);
			return;
		}

		// Update the text
		textScore.text = string.Format ("MARIO\n{0:0000000}", score);
	}

	public void ChangeCoinText(int coins)
	{
		// Make sure a text object is assigned
		if (!textCoin) 
		{
			Debug.LogError ("No textCoin assigned!", this);
			return;
		}

		// Update the text
		textCoin.text = string.Format ("x{0:00}", coins);
	}

	public void ChangeTimeText(int time)
	{
		// Make sure a text object is assigned
		if (!textTime) 
		{
			Debug.LogError ("No textTime assigned!", gameObject);
			return;
		}

		// Update the text
		textTime.text = string.Format ("TIME\n{0}", time);
	}

	public void ChangeLevelText(string level)
	{
		// Make sure a text object is assigned
		if (!textLevel) 
		{
			Debug.LogError ("No textLevel assigned!", this);
			return;
		}

		// Update the text
		textLevel.text = string.Format ("WORLD\n{0}", level);
	}

	public void PopRewardText(Vector3 pos, string rewardText)
	{
		// Displays a text object with the specified text at the position
		GameObject textObj = Instantiate (rewardPrefab) as GameObject;
		textObj.GetComponent<RewardText>().Initialize (rewardText, pos, rewardCanvasTransform);
	}
}