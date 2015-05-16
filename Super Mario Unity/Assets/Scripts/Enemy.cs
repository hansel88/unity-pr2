using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	public bool hasDeathAnimation = true;
	public bool isDying = false;

	void Update()
	{
		HorizontalMovement ();
		//boxCollider.enabled = !isDying;
		if (boxCollider.enabled && isDying)
		{
			boxCollider.enabled = false;
		}
	}

	public void Die()
	{
		if (isDying) return;

		isDying = true;

		// Disable the colliders
		boxCollider.enabled = true;

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
		GM.instance.charManager.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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