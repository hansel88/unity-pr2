using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
	[SerializeField]private GameObject fireflowerProjectilePrefab; // The prefab for the fireflower projectile
	[SerializeField]private Transform fireflowerShootPosition; // Transform for where the fireflowerprojectiles will spawn
	public Transform spriteTransform; // The players sprite object
	[SerializeField]private float fireRate = 1f; // Firerate of the fireflower shooting
	[SerializeField]private LayerMask blockRaycastMask; // Layermask for raycasting blocks
    public GameObject marioDieSound;
    public GameObject powerUpSound;
    public GameObject shootFireballSound;
	
	[HideInInspector]public PlayerState curState; // Current player state
	[HideInInspector]public bool hasStar = false; // If we have a star or not
	[HideInInspector]public bool isInvincible = false; // If we are invincible or not
	private SpriteRenderer spriteRenderer; // Spriterenderer of the sprite object
	private Animator anim;
	private BoxCollider2D charCollider;
	[SerializeField]private BoxCollider2D groundTriggerCollider; // Collider for ground checking
	private CharacterMovement charMove;
	private bool isDying = false; // To check if we are in the process of dying
	private float fireTimer; // Timer for firerate
	[HideInInspector]public int fireflowerCount = 0; // Number of fireflower projectiles in play
	private float starTimer; // Timer for the star powerup
	[HideInInspector]public bool hasHitBlock = false; // To check if we already hit a block this jump 

	void Awake()
	{
		// Stop if a spritetransform isn't assigned
		if (!spriteTransform)
		{
			Debug.LogWarning ("No spritetransform assigned to the player!", this);
			Debug.Break ();
		}

		// Get the component references
		charCollider = GetComponent<BoxCollider2D>();
		charMove = GetComponent<CharacterMovement>();
		anim = spriteTransform.GetComponent<Animator>();
		spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
		fireTimer = fireRate;
	}
	
	void Update()
	{
		// Fire rate timer and shooting
		if (curState == PlayerState.Fireflower && !GM.instance.frozenEntities) // If we have fireflower powerup and the game isn't frozen
		{
			// Check if we are ready to fire
			if (fireTimer < fireRate)
			{
				fireTimer += Time.deltaTime;
			}
			else if (Input.GetButtonDown ("Shoot") && fireflowerCount < 2) // Only allow shooting when there are less than 2 projectiles out
			{
				ShootFireflower ();
			}
		}

		// Star timer
		if (hasStar)
		{
			if (starTimer > 0f)
			{
				starTimer -= Time.deltaTime;
			}
			else
			{
				EndStarPowerup ();
			}
		}
	}
	private float[] offset = new float[]{0f, 0.08f, -0.08f};

	void OnCollisionEnter2D(Collision2D other)
	{
		// Check if we hit block
		if (other.collider.CompareTag (Tags.block))
		{
			// Stop if block is underneath us
			Vector3 pos = transform.position;
			if (other.transform.position.y < pos.y)  return;

			// Set the origin height of the raycast (depening on the size of the player)
			float yOrigin = pos.y + (curState == PlayerState.Small ? 0.08f : 0.16f);

			// Get the distance from the block
			float distance = Vector2.Distance (transform.position, other.transform.position);

			// Raycast three times (from the center of the player, and on both sides), that way we cover the whole width of the player
			for (int i = 0; i < offset.Length; i ++)
			{
				// Set the origin of the raycast (
				Vector2 origin = new Vector2(pos.x + offset[i], yOrigin);

				// Raycast
				RaycastHit2D hit = Physics2D.Raycast (origin, Vector2.up, distance, blockRaycastMask);
				if (hit.collider)
				{
					// Send the hit message to the block we hit
					hit.collider.SendMessage ("OnHit", this, SendMessageOptions.DontRequireReceiver);

					// Stop because we already hit a block this jump, so no need to raycast more
					break;
				}
			}
		}
	}

	void ShootFireflower()
	{
        //Play fireball sound
        Destroy(GameObject.Instantiate(shootFireballSound), 2);

		// Trigger the animation
		anim.SetTrigger ("FireflowerShoot");
		// Increase the projectile count
		fireflowerCount ++;
		// Instantiate and initialize the projectile
		GameObject proj = Instantiate (fireflowerProjectilePrefab, fireflowerShootPosition.position, Quaternion.identity) as GameObject;
		proj.GetComponent<FireflowerProjectile>().Initialize (charMove.facingRight);
		// Reset the firetimer
		fireTimer = 0f;
	}

	public void OnEnemyHit()
	{
		// Stop if we are invincible 
		if (isInvincible)
		{
			return;
		}

		// Freeze all entities
		GM.instance.FreezeEntities ();

		// Set the animationtriggers
		SetAnimationTriggers (false);

		// Make us invincible
		isInvincible = true;

		// Check what state we are in
		if (curState == PlayerState.Small)
		{
			// Die if we are small
			StartCoroutine (Die (true));
		}
		else
		{
			// Go to small
			curState = PlayerState.Small;
		}

		// Set the collider size since we changed size
		SetColliderSize ();

		// If we aren't dying, make us invincible
		if (!isDying)
		{
			StartCoroutine (FlashInvincible (3));
		}
	}
	
	public void PowerUpgrade(PlayerState toState)
	{
		// Check what state we are in
		if ((int)curState > (int)toState) // Pickup is a state lower than us
		{
			// Award score to player
			int scoreReward = 1000;
			GM.instance.Score += scoreReward;
			GUIManager.instance.PopRewardText (transform.position, scoreReward.ToString ());
		}
		else
		{
			// Play powerup sound
            Destroy(GameObject.Instantiate(powerUpSound), 2);

			// If the pickup is a mushroom and we already have a mushrom
			if (curState == PlayerState.Mushroom && toState == PlayerState.Mushroom)
			{
				// Set the state to fireflower
				toState = PlayerState.Fireflower;
			}

			// Only freeze entites when we don't have star and we aren't goin to fireflower
			if (!hasStar && toState != PlayerState.Fireflower)
			{
				GM.instance.FreezeEntities ();
			}

			// Set the state
			curState = toState;

			// Set the collider sizes
			SetColliderSize ();

			// set the animationtriggers
			SetAnimationTriggers (true);
		}
	}

	public void PickupStarPowerup()
	{
		// Activate the starpowerup
		hasStar = true;
		starTimer = 12f;
		SetAnimationTriggers (true);
	}

	void EndStarPowerup()
	{
		// Deactivate the starpowerup
		hasStar = false;
		starTimer = 0f;
		anim.SetInteger ("PlayerState", (int)curState);
		anim.SetBool ("HasStar", hasStar);
	}

	void SetAnimationTriggers(bool poweringUp)
	{
		// Set the animationtriggers needed for the state animations
		anim.SetBool ("PoweringUp", poweringUp);
		anim.SetInteger ("PlayerState", (int)curState);
		anim.SetBool ("HasStar", hasStar);
		anim.SetTrigger ("PowerupTrigger");
	}
	
	IEnumerator FlashInvincible(int seconds)
	{
		// Set the layer so we aren't colliding with entities while invincible
		gameObject.layer = LayerMask.NameToLayer ("InvinciblePlayer");

		float waitTime = 0.2f; // Time to wait between flashes
		
		// Flashing loop
		for (int i = 0; i < seconds / waitTime; i ++)
		{
			ToggleSpriteVisibility (i % 2 == 0);
			yield return new WaitForSeconds(0.2f);
		}
		
		// Make sure player is fully visible when finished with the invincibility
		ToggleSpriteVisibility (true);

		// End the invincibility
		isInvincible = false;
		gameObject.layer = LayerMask.NameToLayer ("Player");
	}
	
	public IEnumerator Die(bool withAnimation)
	{
		// If we are already dying, don't die again
		if (isDying) yield return null;

		// Set that we are dying so we can't double die
		isDying = true;
		
		// Disable charactercolliders
		charCollider.enabled = false;
		groundTriggerCollider.enabled = false;

		
		// Tell GM we are dead
		GM.instance.PlayerIsAlive = false;
		GM.instance.Lives --;

		// Animate death
		if (withAnimation)
		{
			anim.SetTrigger ("DeathTrigger");
		}
        GM.instance.source.Stop();
        Destroy(GameObject.Instantiate(marioDieSound), 4);

		// Wait some time before going to deathscreen
        yield return new WaitForSeconds(3f);
		//yield return new WaitForSeconds(withAnimation ? 2f : 1f);
		//Application.LoadLevel (Application.loadedLevel);
		GM.instance.CheckGameOver ();
	}
	
	void SetColliderSize()
	{
		// Check what state we are in and set the collider settings appropriately
		if (curState == PlayerState.Small)
		{
			charCollider.size = new Vector2(0.16f, 0.16f);
			charCollider.offset = new Vector2(0f, 0.08f);
		}
		else
		{
			charCollider.size = new Vector2(0.16f, 0.32f);
			charCollider.offset = new Vector2(0f, 0.16f);
		}
	}
	
	void ToggleSpriteVisibility(bool fullVisible)
	{
		// Toggle the sprite alpha depending on the bool parameter
		Color col = spriteRenderer.color;
		col.a = fullVisible ? 1f : 0.5f;
		spriteRenderer.color = col;
	}

	public void OnHeadHit(Collider2D other)
	{
		// Check if we hit a block
		if (other.CompareTag (Tags.block))
		{
			// Try to get the block component
			BlockPowerup bPowerup = other.GetComponent<BlockPowerup>();

			// Check if the component is null (if so get the other block component)
			if (bPowerup)
			{
				bPowerup.OnHit (this);
			}
			else
			{
				other.GetComponent<BlockBrick>().OnHit (this);
			}
		}
	}

	public IEnumerator AnimatePipeEntering()
	{
		transform.SetParent (GM.instance.transform);
		// animate
		// wait for animation
		yield return new WaitForEndOfFrame();
	}
}

// The states the player can be in
public enum PlayerState
{
	Small,
	Mushroom,
	Fireflower
}