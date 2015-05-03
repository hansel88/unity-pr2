using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
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
		if (anim)
		{
			anim.SetTrigger ("DeathTrigger");
		}
	}

	public void DestroyEntity()
	{
		gameObject.SetActive (false);
	}
}