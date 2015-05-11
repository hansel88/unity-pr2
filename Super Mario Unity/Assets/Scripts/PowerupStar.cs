using UnityEngine;
using System.Collections;

public class PowerupStar : Powerup
{
	[SerializeField]private Vector2 movementVelocity = new Vector2(2f, 2f);
	private Vector2 curVel;
	private float normalThreshold = 0.5f;
	private Vector2 savedVelocity;

	void Update()
	{
		SetCurVel ();

		if (isActive)
		{
			if (GM.instance.frozenEntities && !rBody.isKinematic)
			{
				savedVelocity = rBody.velocity;
				rBody.isKinematic = true;
			}
			else if (!GM.instance.frozenEntities && rBody.isKinematic)
			{
				rBody.velocity = savedVelocity;
				rBody.isKinematic = false;
			}
		}
	}
	
	public void OnCollisionEnter2D(Collision2D other)
	{
		//if (!isActive) return;

		float normalX = other.contacts[0].normal.x;
		if (other.collider.CompareTag (Tags.player))
		{
			other.collider.GetComponent<CharacterManager>().PickupStarPowerup ();
			OnPickup ();
		}
		else if (normalX > normalThreshold || normalX < -normalThreshold)
		{
			ChangeDirectionOnCollision (other);
		}
		else if (!GM.instance.frozenEntities && !rBody.isKinematic)
		{
			SetCurVel ();
		}
		rBody.velocity = curVel;
	}

	void SetCurVel()
	{
		curVel = new Vector2(movementVelocity.x * direction, movementVelocity.y);
	}

	public void Initialize(bool moveRight)
	{
		movementVelocity.x *= moveRight ? 1 : -1;
		curVel = movementVelocity;
		rBody.velocity = curVel;
	}

	public override void OnPickup()
	{
		base.OnPickup ();

		GM.instance.HasStar = true;
	}
}