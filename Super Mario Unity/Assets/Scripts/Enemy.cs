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
		RewardScore ();
		gameObject.SetActive (false); // TODO animate death and remove this
	}
}