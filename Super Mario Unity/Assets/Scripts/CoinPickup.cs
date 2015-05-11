using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag (Tags.player))
		{
			GM.instance.Coins ++;
			gameObject.SetActive (false);
		}
	}
}