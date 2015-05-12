﻿using UnityEngine;
using System.Collections;

public class BlockBrick : MonoBehaviour
{
	public GameObject brickParticlePrefab;
	public Sprite[] brickParticles;
	public float particleForce = 10f;
	public Vector2[] particleVectors = new Vector2[4];
	public int activateReward = 0;

	public void OnHit(CharacterManager charManager)
	{
		if (charManager.hasHitBlock) return;
		charManager.hasHitBlock = true;

		GM.instance.Score += activateReward;

		for (int i = 0; i < brickParticles.Length; i ++)
		{
			GameObject obj = Instantiate (brickParticlePrefab, transform.position, Quaternion.identity) as GameObject;
			obj.GetComponent<SpriteRenderer>().sprite = brickParticles[i];
			obj.GetComponent<Rigidbody2D>().AddForce (particleVectors[i] * particleForce);
		}
		gameObject.SetActive (false);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		/*if (other.collider.CompareTag (Tags.player))
		{
			CharacterManager charManager = other.collider.GetComponent<CharacterManager>();
			if (charManager.ValidHeadHit (other.contacts[0].point, charManager.transform.position, charManager.charCollider.size,
			                              charManager.charCollider.offset, charManager.charCollider.bounds) 
			    && !charManager.hasHitBlock && charManager.curState != PlayerState.Small)
			{
				charManager.hasHitBlock = true;
				OnHit ();
			}
		}
		else if (other.collider.CompareTag (Tags.enemy))
		{
			if (other.collider.GetComponent<EnemyShell>())
			{
				OnHit ();
			}
		}*/
	}
}