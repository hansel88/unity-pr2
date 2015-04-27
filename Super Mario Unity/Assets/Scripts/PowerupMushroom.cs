using UnityEngine;
using System.Collections;

public class PowerupMushroom : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();

		// Check if the player already has mushroom
		if (GM.instance.HasMushroom)
		{
			// Give the player some points
			RewardScore ();
		}
		else
		{
			GM.instance.HasMushroom = true;
		}
	}
}