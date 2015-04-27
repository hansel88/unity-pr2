using UnityEngine;
using System.Collections;

public class PowerupMushroom : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();

		GM.instance.HasMushroom = true;
	}
}