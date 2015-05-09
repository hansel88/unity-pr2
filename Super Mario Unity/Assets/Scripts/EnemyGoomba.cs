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
		if (other.collider.CompareTag (Tags.player))
		{
			// Check with the player to make sure this wasnt a headhit (player jumped on this enemy)
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.ValidHeadHit (other.contacts[0].point, boxCollider))
			{
				JumpedOn ();
				Die ();
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
}