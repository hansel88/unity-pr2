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

			if (charManager.ValidHeadHit (other.contacts[0].point, boxCollider))
			{
				JumpedOn ();
				Instantiate (shellPrefab, transform.position, Quaternion.identity);
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