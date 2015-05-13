using UnityEngine;
using System.Collections;

public class EnemyKoopaTroopa : Enemy
{
	public GameObject shellPrefab;

	[ContextMenu("Hit")]
	public void OnJumpHit()
	{
		JumpedOn ();
		Instantiate (shellPrefab, transform.position, Quaternion.identity);
		Die ();
	}

	public void OnCollide(Transform other)
	{
		other.SendMessage ("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
		//base.OnCollide (other);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			// Check with the player to make sure this wasnt a headhit (player jumped on this enemy)
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.hasStar)
			{
				InstaDeath (200);
				return;
			}

			/*if (charManager.ValidHeadHit (other.contacts[0].point, boxCollider))
			{
				JumpedOn ();
				Instantiate (shellPrefab, transform.position, Quaternion.identity);
				Die ();
			}
			else
			{
				charManager.OnEnemyHit ();
			}*/
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
			ChangeDirectionOnCollision (other);
		}
	}

	public override void OnHeadHit(Collider2D other)
	{
		// Stop colliding if player is already hit
		if (other.CompareTag (Tags.player))
		{
			CharacterManager charManager = other.GetComponent<CharacterManager>();
			if (charManager.isInvincible) return;
			charManager.GetComponent<CharacterMovement>().Jump (true);
		}
		
		base.OnHeadHit (other);
		OnJumpHit ();
	}
}