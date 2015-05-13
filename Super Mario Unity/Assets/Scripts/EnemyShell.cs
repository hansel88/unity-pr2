using UnityEngine;
using System.Collections;

public class EnemyShell : Enemy
{
	public int scoreMovingReward = 100;

	public override void Start()
	{
		base.Start ();

		headCollider.enabled = false;

		// Start with no movement
		direction = 0;
		StartCoroutine (EnableHead ());
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			// Check with the player to make sure this wasnt a headhit (player jumped on this enemy)
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.hasStar)
			{
				InstaDeath (200);
				return;
			}

			/*if (charManager.ValidHeadHit (other.contacts[0].point, boxCollider))
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
			else
			{
				if (direction == 0)
				{
					direction = other.transform.position.x > transform.position.x ? -1 : 1;
					RewardScore ();
				}
				else
				{
					charManager.OnEnemyHit ();
				}
			}*/
			if (direction == 0)
			{
				direction = other.transform.position.x > transform.position.x ? -1 : 1;
				RewardScore ();
			}
			else
			{
				charManager.OnEnemyHit ();
			}
		}
		else if (other.collider.CompareTag (Tags.enemy) && direction != 0)
		{
			other.collider.GetComponent<Enemy>().InstaDeath (800);
		}
		else
		{
			ChangeDirectionOnCollision (other);
		}
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
			print ("Jumped on and shell is starting");
			// Reward player with score
			RewardScore ();
			// Start moving shell
			direction = Random.value > 0.5f ? 1 : -1; // Randomized direction
		}
		else // Is moving
		{
			print ("Jumped on and shell is stopping");
			// Reward player with score
			RewardScore (scoreMovingReward);
			// Stop shell
			direction = 0;
		}
	}

	/*public void OnCollide(Transform other)
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
	}*/

	public override void OnHeadHit(Collider2D other)
	{
		base.OnHeadHit (other);
		OnJumpHit ();
	}

	public void OnShellCollision(Transform other)
	{
		other.SendMessage ("InstaDeath", SendMessageOptions.DontRequireReceiver);
	}
}