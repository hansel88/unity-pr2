using UnityEngine;
using System.Collections;

public class PowerupStar : Powerup
{
	public override void OnPickup()
	{
		base.OnPickup ();

		GM.instance.HasStar = true;
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