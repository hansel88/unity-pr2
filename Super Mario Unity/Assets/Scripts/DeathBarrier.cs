using UnityEngine;
using System.Collections;

// Kills stuff when touched (for when stuff falls through holes)
public class DeathBarrier : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag (Tags.player))
		{
			print ("deathbarrer die");
			StartCoroutine (other.gameObject.GetComponent<CharacterManager>().Die (false));
		}
		else
		{
			Destroy (other.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag (Tags.player))
		{
			print ("deathbarrer die");
			StartCoroutine (other.GetComponent<CharacterManager>().Die (false));
		}
		else
		{
			Destroy (other.gameObject);
		}
	}
}