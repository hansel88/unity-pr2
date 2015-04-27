using UnityEngine;
using System.Collections;

public class PowerupFireflower : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();

		// Check if the player already has fireflower
		if (GM.instance.HasFireFly)
		{
			// Give the player some points
			RewardScore ();
		}
		else
		{
			GM.instance.HasFireFly = true;
		}
	}
}