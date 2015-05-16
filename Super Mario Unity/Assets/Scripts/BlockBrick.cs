using UnityEngine;
using System.Collections;

// Functionality of the brick block
public class BlockBrick : MonoBehaviour
{
	public GameObject brickParticlePrefab; // Prefab for the particles
	public Sprite[] brickParticles; // The different sprites for the particles
	public float particleForce = 10f; // Force of the particles
	public Vector2[] particleVectors = new Vector2[4]; // Particle directions
	public int activateReward = 0; // Score reward for activating
    public GameObject blockBreakSound;
    public GameObject bumpSound;

	private Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OnHit(CharacterManager charManager)
	{
		if (charManager)
		{
			// Stop if we already hit a block this jump
			if (charManager.hasHitBlock) return;
			charManager.hasHitBlock = true;

			// If we are small, just thump the block
			if (charManager.curState == PlayerState.Small)
			{
				anim.SetTrigger ("ActivateTrigger");
                Destroy(GameObject.Instantiate(bumpSound), 2);
				return;
			}
		}

		// Reward player
		GM.instance.Score += activateReward;

		// Fire of the particles
		for (int i = 0; i < brickParticles.Length; i ++)
		{
            Destroy(GameObject.Instantiate(blockBreakSound), 2);
			GameObject obj = Instantiate (brickParticlePrefab, transform.position, Quaternion.identity) as GameObject;
			obj.GetComponent<SpriteRenderer>().sprite = brickParticles[i];
			obj.GetComponent<Rigidbody2D>().AddForce (particleVectors[i] * particleForce);
		}

		// Deactivate the object
		gameObject.SetActive (false);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// If enemy hit
		if (other.collider.CompareTag (Tags.enemy))
		{
			// If enemy is shell
			if (other.collider.GetComponent<EnemyShell>())
			{
				// Hit block
				OnHit (null);
			}
		}
	}
}