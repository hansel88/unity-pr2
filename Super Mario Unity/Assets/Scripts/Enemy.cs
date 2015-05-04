using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	public bool hasDeathAnimation = true;

	public override void Update()
	{
		base.Update ();
		HorizontalMovement ();
	}

	public void Die()
	{
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

	public void DestroyEntity()
	{
		gameObject.SetActive (false);
	}
}