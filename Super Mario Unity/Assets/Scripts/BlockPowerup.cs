using UnityEngine;
using System.Collections;

// Functionality of the powerup block
public class BlockPowerup : MonoBehaviour
{
	public enum BlockContent {Coin, Mushroom, Fireflower, Star, OneUp}; // Content of the block
	public BlockContent content = BlockContent.Coin; // Default to coin
	public int activateCount = 1; // Number of times the block can be hit before being disabled
	public GameObject[] itemPrefabs; // Prefab for the contents
	public Transform itemMoverParent; // Transform used to move the items
	private bool isActive = true; // Whether the block is hittable or not
	private int curActivateCount; // Number of times the block has been hit
	private Powerup curPowerup; // Spawned powerup
	private Animator anim;
    public GameObject powerUpAppearSound;
	
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
		}

		// Set active state and stop if the block isn't active
		isActive = curActivateCount < activateCount;
		if (!isActive) return;
		curActivateCount ++;

		// Play SFX
        Destroy(GameObject.Instantiate(powerUpAppearSound), 2);

		// Check which content the block has
		if (content == BlockContent.Coin)
		{
			// Spawn coing
			Instantiate (itemPrefabs[(int)content], itemMoverParent.position, Quaternion.identity);
		}
		else
		{
			// Change item to fireflower if player already has mushroom or fireflower
			if (content == BlockContent.Mushroom && (int)GM.instance.charManager.curState >= (int)PlayerState.Mushroom) 
			{
				content = BlockContent.Fireflower;
			}

			// Spawn item
			GameObject powerupObj = Instantiate (itemPrefabs[(int)content], itemMoverParent.position, Quaternion.identity) as GameObject;
			curPowerup = powerupObj.GetComponent<Powerup>();

			// Disable so it doesn't move before it has popped up
			curPowerup.canMove = false;
			curPowerup.boxCollider.enabled = false;
			curPowerup.isActive = false;
			curPowerup.rBody.isKinematic = true;
			powerupObj.transform.SetParent (itemMoverParent);
		}

		// Set the animator states
		anim.SetBool ("DisableBlock", curActivateCount >= activateCount);
		anim.SetTrigger ("ActivateTrigger");
	}

	public void StartPowerup()
	{
		// If we have a powerup spawned, start it up so it moves
		if (curPowerup)
		{
			curPowerup.canMove = true;
			curPowerup.rBody.isKinematic = false;
			curPowerup.isActive = true;
			curPowerup.boxCollider.enabled = true;
			curPowerup.transform.SetParent (null);
			curPowerup = null;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// If hit by enemy
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