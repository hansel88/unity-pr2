using UnityEngine;
using System.Collections;

// Script for the box coin (for when the animation finishes)
public class BoxCoin : MonoBehaviour
{
	public int reward = 200; // Score reward to give player

	public void StopCoin()
	{
		// Reward player and deactivate object
		GM.instance.Coins ++;
		GM.instance.Score += reward;
		GUIManager.instance.PopRewardText (transform.GetChild (0).position, reward.ToString ());
		gameObject.SetActive (false); // TODO Pool
	}
}