using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
	public Transform rewardCanvas;
	public Text textScore;
	public Text textCoin;
	public Text textTime;
	public Text textLevel;
	public GameObject rewardPrefab;

	public static GUIManager instance;
    private bool isPaused = false;

	void Awake()
	{
		instance = this;
	}



    void Update () 
    {
		if (Input.GetKeyDown("p"))
        {
            if(isPaused)
            {
                isPaused = false;
                Time.timeScale = 0;
            }
            else
            {
                isPaused = true;
                Time.timeScale = 1;
            }

        }

	}

	public void ChangeScoreText(int score)
	{
		if (!textScore) 
		{
			Debug.LogError ("No textScore assigned!", this);
			return;
		}

		textScore.text = string.Format ("MARIO\n{0:0000000}", score);
	}

	public void ChangeCoinText(int coins)
	{
		if (!textCoin) 
		{
			Debug.LogError ("No textCoin assigned!", this);
			return;
		}

		textCoin.text = string.Format ("x{0:00}", coins);
	}

	public void ChangeTimeText(int time)
	{
		print ("TimeText: " + textTime);
		if (!textTime) 
		{
			Debug.LogError ("No textTime assigned!", gameObject);
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

	public void PopRewardText(Vector3 pos, string rewardText)
	{
		//GameObject textObj = new GameObject("RewardText", typeof(RewardText));
		//Text text = textObj.GetComponent<Text>(); // TODO Get from poolmanager
		//text.transform.position = pos;
		//text.text = rewardText;
		GameObject textObj = Instantiate (rewardPrefab) as GameObject;
		textObj.GetComponent<RewardText>().Initialize (rewardText, pos, rewardCanvas);
	}
}