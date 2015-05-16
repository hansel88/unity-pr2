using UnityEngine;
using System.Collections;

public class EnemyGoomba : Enemy
{
	public void OnJumpHit()
	{
		Die ();
	}
	
	public void OnCollide(Transform other)
	{
		other.SendMessage ("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
		//base.OnCollide (other);
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (isDying) return;
		
		if (other.collider.CompareTag (Tags.player))
		{
			// Check with the player to make sure this wasnt a headhit (player jumped on this enemy)
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.hasStar)
			{
				InstaDeath (100);
				return;
			}
			
			/*if (charManager.ValidHeadHit (other.contacts[0].point, boxCollider))
			{
				JumpedOn ();
				Die ();
			}
			else
			{
				charManager.OnEnemyHit ();
			}*/
			if (transform.ContactPointIsHead (other.contacts[0].point, 0.003f))
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
			if (GM.instance.frozenEntitiesCooldown) return;
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
			print ("head jump");
			charManager.GetComponent<CharacterMovement>().Jump (true);
		}
		base.OnHeadHit (other);
		OnJumpHit ();
	}
	
	IEnumerator headreset()
	{
		yield return new WaitForSeconds(0.5f);
		//GM.instance.charManager.lastHitHead = false;
	}
}