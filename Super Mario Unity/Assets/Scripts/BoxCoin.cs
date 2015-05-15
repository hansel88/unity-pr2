using UnityEngine;
using System.Collections;

public class BoxCoin : MonoBehaviour
{
	public int reward = 200;

	public void StopCoin()
	{
		GM.instance.Coins ++;
		GM.instance.Score += reward;
		GUIManager.instance.PopRewardText (transform.GetChild (0).position, reward.ToString ());
		gameObject.SetActive (false); // TODO Pool
	}
}