using UnityEngine;
using System.Collections;

public class PowerupFireflower : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();
		
		GM.instance.HasFireFly = true;
	}
}