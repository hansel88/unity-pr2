using UnityEngine;
using System.Collections;

// Script for the Koopa Troopa
public class EnemyKoopaTroopa : Enemy
{
	public GameObject shellPrefab;

	public void OnJumpHit()
	{
		JumpedOn ();
		Instantiate (shellPrefab, transform.position, Quaternion.identity);
		Die ();
	}

	public void OnCollide(Transform other)
	{
		other.SendMessage ("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			// Die if the player has star
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.hasStar)
			{
				InstaDeath (200);
				return;
			}

			// Check if we hit the head
			if (transform.ContactPointIsHead (other.contacts[0].point, 0.003f, -0.04f))
			{
				OnHeadHit (other.collider);
			}
			else
			{
				charManager.OnEnemyHit ();
			}
		}
		else
		{
			// Check if we just unfroze before turning (to prevent collision event disabling the kinematic option)
			if (GM.instance.frozenEntitiesCooldown) return;
			ChangeDirectionOnCollision (other);
		}
	}

	public void OnHeadHit(Collider2D other)
	{
		// Stop colliding if player is already hit
		if (other.CompareTag (Tags.player))
		{
			CharacterManager charManager = other.GetComponent<CharacterManager>();
			if (charManager.isInvincible) return;
			charManager.GetComponent<CharacterMovement>().Jump (true);
		}
		
		OnJumpHit ();
	}
}