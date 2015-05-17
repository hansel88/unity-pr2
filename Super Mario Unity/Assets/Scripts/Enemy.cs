using UnityEngine;
using System.Collections;

// The script that all enemies inherits from. Handles movement, jumped on event and death
public class Enemy : Entity
{
	public bool hasDeathAnimation = true; // Whether to show the death animation or not
	public bool isDying = false;

	void Update()
	{
		// Move the enemy
		HorizontalMovement ();

		// Disable the collider if we are dying
		if (boxCollider.enabled && isDying)
		{
			boxCollider.enabled = false;
		}
	}

	public void Die()
	{
		// Don't die again if we already are dying
		if (isDying) return;
		isDying = true;

		// Disable the colliders
		boxCollider.enabled = true;

		// Stop moving
		canMove = false;

		// Reward player
		RewardScore ();

		// Play death animation or destroy the enemy
		if (hasDeathAnimation)
		{
			if (anim)
			{
				anim.SetTrigger ("DeathTrigger");
			}
		}
		else
		{
			DestroyEntity ();
		}
	}

	public void JumpedOn()
	{
		// Jump the player
		GM.instance.charManager.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GM.instance.charManager.GetComponent<CharacterMovement>().Jump (true);
	}

	public void DestroyEntity()
	{
		gameObject.SetActive (false);
	}

	public void InstaDeath(int pointReward)
	{
		// Kill the enemy
		anim.SetTrigger ("InstaDeathTrigger");
		RewardScore (pointReward);
		canMove = false;
	}
}