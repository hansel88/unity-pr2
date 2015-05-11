using UnityEngine;
using System.Collections;

public class PowerupLife : Powerup
{
	public override void OnPickup()
	{
		if (!isActive) return;

		base.OnPickup ();
		
		GM.instance.Lives ++;
		GUIManager.instance.PopRewardText(transform.position, "+1up");
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