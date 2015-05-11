using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	public bool hasDeathAnimation = true;
	public BoxCollider2D headCollider; // The head cillider
	public bool isDying = false;
	private bool hasJumped = false;

	void Update()
	{
		HorizontalMovement ();
	}

	public void Die()
	{
		if (isDying) return;

		isDying = true;

		// Disable the colliders
		boxCollider.enabled = false;

		canMove = false;
		RewardScore ();

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
		if (hasJumped) return;

		hasJumped = true;
		GM.instance.charManager.GetComponent<CharacterMovement>().Jump (true);
	}

	public void DestroyEntity()
	{
		gameObject.SetActive (false);
	}

	public void InstaDeath(int pointReward)
	{
		anim.SetTrigger ("InstaDeathTrigger");
		RewardScore (pointReward);
		canMove = false;
	}
}