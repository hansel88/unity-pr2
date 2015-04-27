using UnityEngine;
using System.Collections;

public class PowerupStar : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();

		GM.instance.HasStar = true;
	}
}