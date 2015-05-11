using UnityEngine;
using System.Collections;

public class BlockBrick : Block
{
	public int activateReward = 0;

	public void OnHit()
	{
		print ("onhti");
		GM.instance.Score += activateReward;

		base.OnActivate ();
		gameObject.SetActive (false);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag (Tags.player))
		{
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.ValidHeadHit (other.contacts[0].point, charManager.transform.position, charManager.charCollider.size,
			                              charManager.charCollider.offset, charManager.charCollider.bounds))
			{
				OnHit ();
			}
		}
		else
		{
			
		}
	}
}