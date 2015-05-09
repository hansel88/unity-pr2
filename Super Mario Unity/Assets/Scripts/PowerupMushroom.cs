﻿using UnityEngine;
using System.Collections;

public class PowerupMushroom : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();

		GM.instance.charManager.PowerUpgrade (PlayerState.Mushroom);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			OnPickup ();
		}
		else
		{
			ChangeDirectionOnCollision (other);
		}
	}
}