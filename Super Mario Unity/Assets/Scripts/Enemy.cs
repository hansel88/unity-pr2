using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	public bool hasDeathAnimation = true;
	public BoxCollider2D headCollider; // The head cillider

	public override void Update()
	{
		base.Update ();
		HorizontalMovement ();
	}

	public void Die()
	{
		// Disable the colliders
		boxCollider.enabled = false;
		headCollider.enabled = false;

		canMove = false;
		RewardScore ();
		//gameObject.SetActive (false); // TODO animate death and remove this

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

	public override IEnumerator ChangeDirection(bool otherIsPlayer)
	{
		if (otherIsPlayer && GM.instance.charManager.isInvincible)
		{
			yield return null;
		}
		StartCoroutine (base.ChangeDirection (otherIsPlayer));
	}

	public void DestroyEntity()
	{
		gameObject.SetActive (false);
	}

	public void InstaDeath()
	{
		anim.SetTrigger ("InstaDeathTrigger");
	}
}