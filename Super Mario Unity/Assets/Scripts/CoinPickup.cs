using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{
    public GameObject coinPickupSound;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag (Tags.player))
		{
			// Play the sound effect and increment the coin amount of the player
            Destroy(GameObject.Instantiate(coinPickupSound), 2);
			GM.instance.Coins ++;
			gameObject.SetActive (false);
		}
	}
}