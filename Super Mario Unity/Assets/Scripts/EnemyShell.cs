using UnityEngine;
using System.Collections;

public class EnemyShell : Enemy
{
	public int scoreMovingReward = 100;

	public override void Start()
	{
		base.Start ();

		// Start with no movement
		direction = 0;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			// Die if the player has star
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.hasStar)
			{
				InstaDeath (200);
				return;
			}

			// Check if we hit the head
			if (transform.ContactPointIsHead (other.contacts[0].point, 0.003f))
			{
				OnHeadHit (other.collider);
			}
			else
			{
				if (direction == 0) // If the shell isn't moving
				{
					// Start moving the shell
					direction = other.transform.position.x > transform.position.x ? -1 : 1;
					RewardScore ();
				}
				else
				{
					// Fire enemyhit event
					charManager.OnEnemyHit ();
				}
			}
		}
		else if (other.collider.CompareTag (Tags.enemy) && direction != 0) // If an enemy hit the shell while its moving
		{
			// Kill the enemy
			other.collider.GetComponent<Enemy>().InstaDeath (800);
		}
		else
		{
			// Check if we just unfroze before turning (to prevent collision event disabling the kinematic option)
			if (GM.instance.frozenEntitiesCooldown) return;
			ChangeDirectionOnCollision (other);
		}
	}

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

	public void OnHeadHit(Collider2D other)
	{
		// Stop colliding if player is already hit
		if (other.CompareTag (Tags.player))
		{
			CharacterManager charManager = other.GetComponent<CharacterManager>();
			if (charManager.isInvincible) return;
			charManager.GetComponent<CharacterMovement>().Jump (true);
		}

		OnJumpHit ();
	}

	public void OnShellCollision(Transform other)
	{
		other.SendMessage ("InstaDeath", SendMessageOptions.DontRequireReceiver);
	}
}