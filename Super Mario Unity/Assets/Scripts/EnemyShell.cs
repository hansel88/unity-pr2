using UnityEngine;
using System.Collections;

public class EnemyShell : Enemy
{
	public int scoreMovingReward = 100;

	void Start()
	{
		headCollider.enabled = false;

		// Start with no movement
		direction = 0;
		StartCoroutine (EnableHead ());
	}

	IEnumerator EnableHead()
	{
		yield return new WaitForSeconds(0.1f);
		headCollider.enabled = true;
	}

	[ContextMenu("Hit")]
	public void OnJumpHit()
	{
		JumpedOn ();

		// Check if shell is moving
		if (direction == 0) // Is not moving
		{
			// Reward player with score
			RewardScore ();
			// Start moving shell
			direction = Random.value > 0.5f ? 1 : -1; // Randomized direction
		}
		else // Is moving
		{
			// Reward player with score
			RewardScore (scoreMovingReward);
			// Stop shell
			direction = 0;
		}
	}

	public void OnCollide(Transform other)
	{
		if (other.CompareTag (Tags.enemy) && direction != 0)
		{
			OnShellCollision (other);
			// TODO Dont change the shell direction here
		}

		// TODO If other == player, dont change direction
		if (direction == 0 && !other.CompareTag (Tags.enemy))
		{
			print ("changdir");
			direction = other.position.x > transform.position.x ? -1 : 1;
			RewardScore ();
		}
		else
		{
			other.SendMessage ("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnShellCollision(Transform other)
	{
		other.SendMessage ("InstaDeath", SendMessageOptions.DontRequireReceiver);
	}
}