using UnityEngine;
using System.Collections;

public class PowerupFireflower : Powerup
{
	public override void OnPickup()
	{
		if (!isActive) return;

		base.OnPickup ();

		GM.instance.charManager.PowerUpgrade (PlayerState.Fireflower);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			OnPickup ();
		}
	}
}