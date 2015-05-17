using UnityEngine;
using System.Collections;

// The star powerup (handles bouncing)
public class PowerupStar : Powerup
{
	[SerializeField]private Vector2 movementVelocity = new Vector2(2f, 2f); // Bounce vector
	private Vector2 curVel;
	private float normalThreshold = 0.5f; // Threshold for what it considers a vertical wall (what to turn around on)
	private Vector2 savedVelocity;

	void Update()
	{
		SetCurVel ();

		// (Un)freeze and keep velocity when (un)freezing the game
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
		// Get the normal
		float normalX = other.contacts[0].normal.x;

		// Check what we hit
		if (other.collider.CompareTag (Tags.player)) // Hit player
		{
			// Register pickup
			other.collider.GetComponent<CharacterManager>().PickupStarPowerup ();
			OnPickup ();
		}
		else if (normalX > normalThreshold || normalX < -normalThreshold) // Hit vertical wall
		{
			// Turn around
			ChangeDirectionOnCollision (other);
		}
		else if (!GM.instance.frozenEntities && !rBody.isKinematic) // Bounced
		{
			SetCurVel ();
		}

		// Set new velocity
		rBody.velocity = curVel;
	}

	void SetCurVel()
	{
		curVel = new Vector2(movementVelocity.x * direction, movementVelocity.y);
	}

	public void Initialize(bool moveRight)
	{
		// Starts the star in a moveRight direction
		movementVelocity.x *= moveRight ? 1 : -1;
		curVel = movementVelocity;
		rBody.velocity = curVel;
	}

	public override void OnPickup()
	{
		base.OnPickup ();
	}
}