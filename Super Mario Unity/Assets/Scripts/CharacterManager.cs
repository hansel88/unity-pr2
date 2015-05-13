using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
	public PlayerState curState;
	public bool hasStar = false;
	public bool isInvincible = false;
	public GameObject fireflowerProjectilePrefab;
	public Transform fireflowerShootPosition;
	public Transform headCollider;

	public Transform spriteTranform;
	[HideInInspector]public Animator anim;
	public BoxCollider2D charCollider;
	private CharacterMovement charMove;
	private SpriteRenderer spriteRenderer;
	private bool isDying = false;
	private float fireTimer;
	[SerializeField]private float fireRate = 1f;
	public int fireflowerCount = 0;
	private float starTimer;
	public bool hasHitBlock = false;

    public GameObject marioDieSound;
    public GameObject powerUpSound;

	void Awake()
	{
		if (!spriteTranform)
		{
			Debug.LogWarning ("No spritetransform assigned to the player!", this);
			Debug.Break ();
		}
		charCollider = GetComponent<BoxCollider2D>();
		charMove = GetComponent<CharacterMovement>();
		anim = spriteTranform.GetComponent<Animator>();
		spriteRenderer = spriteTranform.GetComponent<SpriteRenderer>();
		fireTimer = fireRate;
	}
	
	void Update()
	{
		if (curState == PlayerState.Fireflower && !GM.instance.frozenEntities)
		{
			if (fireTimer < fireRate)
			{
				fireTimer += Time.deltaTime;
			}
			else if (Input.GetButtonDown ("Shoot") && fireflowerCount < 2)
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

		//ValidHeadHit (Vector3.zero, charCollider);
		ValidHeadHit (Vector3.zero, transform.position, charCollider.size, charCollider.offset, charCollider.bounds);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.collider.CompareTag (Tags.enemy) 
		    && !other.collider.CompareTag (Tags.powerup) 
		    && !other.collider.CompareTag (Tags.player))
		{
			Vector3 normal = other.contacts[0].point.normalized;
			//print ("N: " + normal);
			if (normal.x < 0f && normal.y < -0.05f)
			{
				charMove.grounded = true;
				hasHitBlock = false;
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine (v1, v2);
		Gizmos.DrawLine (v2, v4);
		Gizmos.DrawLine (v4, v3);
		Gizmos.DrawLine (v3, v1);
	}
	Vector3 v1;
	Vector3 v2;
	Vector3 v3;
	Vector3 v4;

	public bool ValidHeadHit(Vector2 colPoint, BoxCollider2D headCol, float jumpRectHeight = 0.02f)
	{
		//float jumpRectHeight = 0.02f;
		Vector2 colliderSize = new Vector3 (headCol.size.x * 0.95f, jumpRectHeight);
		Vector3 worldPos = headCol.transform.TransformPoint (headCol.offset);
		Rect jumpRect = new Rect(0f, 0f, colliderSize.x, colliderSize.y);
		jumpRect.center = new Vector2(worldPos.x, worldPos.y + headCol.bounds.extents.y);

		v1 = new Vector3( jumpRect.xMin, jumpRect.yMax, worldPos.z);
		v2 = new Vector3( jumpRect.xMax, jumpRect.yMax, worldPos.z);
		v3 = new Vector3( jumpRect.xMin, jumpRect.yMin, worldPos.z);
		v4 = new Vector3( jumpRect.xMax, jumpRect.yMin, worldPos.z);
		return jumpRect.Contains (colPoint);
	}

	public bool ValidHeadHit(Vector2 colPoint, Vector3 pos, Vector2 size, Vector2 offset, Bounds b, float rectHeight = 0.07f)
	{
		// TODO Optimize this

		//float jumpRectHeight = 0.02f;
		Vector2 colliderSize = new Vector3 (size.x * 0.95f, rectHeight);
		GameObject temp = new GameObject("Temp");
		temp.transform.position = pos;
		Vector3 worldPos = temp.transform.TransformPoint (offset);
		Rect jumpRect = new Rect(0f, 0f, colliderSize.x, colliderSize.y);
		jumpRect.center = new Vector2(worldPos.x, worldPos.y + b.extents.y);
		Destroy (temp);
		
		v1 = new Vector3( jumpRect.xMin, jumpRect.yMax, worldPos.z);
		v2 = new Vector3( jumpRect.xMax, jumpRect.yMax, worldPos.z);
		v3 = new Vector3( jumpRect.xMin, jumpRect.yMin, worldPos.z);
		v4 = new Vector3( jumpRect.xMax, jumpRect.yMin, worldPos.z);
		
		return jumpRect.Contains (colPoint);
	}
	
	void ShootFireflower()
	{
		anim.SetTrigger ("FireflowerShoot");
		fireflowerCount ++;
		GameObject projectile = Instantiate (fireflowerProjectilePrefab, fireflowerShootPosition.position, Quaternion.identity) as GameObject;
		projectile.GetComponent<FireflowerProjectile>().Initialize (charMove.facingRight);
		fireTimer = 0f;
	}

	public void DeductFireflowerCount()
	{
		fireflowerCount --;
		if (fireflowerCount < 0)
		{
			fireflowerCount = 0;
		}
	}
	
	public void OnEnemyHit()
	{
		if (isInvincible)
		{
			return;
		}
		
		GM.instance.FreezeEntities ();
		
		SetAnimationTriggers (false);
		isInvincible = true;

		if (curState == PlayerState.Small)
		{
			StartCoroutine (Die (true));
		}
		else
		{
			curState = PlayerState.Small;
		}

		/*switch(curState)
		{
		case PlayerState.Small:
			StartCoroutine (Die (true));
			break;
		case PlayerState.Mushroom:
			curState = PlayerState.Small;
			break;
		case PlayerState.Fireflower:
			curState = PlayerState.Small;
			break;
		}*/
		
		
		SetColliderSize ();
		
		if (!isDying)
		{
			StartCoroutine (FlashInvincible (3));
		}
	}
	
	public void PowerUpgrade(PlayerState toState)
	{
		if ((int)curState >= (int)toState)
		{
			int scoreReward = 1000;
			GM.instance.Score += scoreReward;
			GUIManager.instance.PopRewardText (transform.position, scoreReward.ToString ());
		}
		else
		{
            Destroy(GameObject.Instantiate(powerUpSound), 2);
			if (curState == PlayerState.Mushroom && toState == PlayerState.Mushroom)
			{
				toState = PlayerState.Fireflower;
			}

			GM.instance.FreezeEntities ();
			curState = toState;
			SetColliderSize ();
			SetAnimationTriggers (true);
		}
	}

	public void PickupStarPowerup()
	{
		hasStar = true;
		starTimer = 12f;
		SetAnimationTriggers (true);
	}

	void EndStarPowerup()
	{
		hasStar = false;
		starTimer = 0f;
		anim.SetInteger ("PlayerState", (int)curState);
		anim.SetBool ("HasStar", hasStar);
	}

	void SetAnimationTriggers(bool poweringUp)
	{
		anim.SetBool ("PoweringUp", poweringUp);
		anim.SetInteger ("PlayerState", (int)curState);
		anim.SetBool ("HasStar", hasStar);
		anim.SetTrigger ("PowerupTrigger");
	}
	
	IEnumerator FlashInvincible(int seconds)
	{
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
		
		isInvincible = false;
		gameObject.layer = LayerMask.NameToLayer ("Player");
	}
	
	public IEnumerator Die(bool withAnimation)
	{
		// If we are already dying, don't die again
		if (isDying) yield return null;
		
		// Set that we are dying so we can't double die
		isDying = true;
		
		// Disable charactercollider
		charCollider.enabled = false;
		
		// Tell GM we are dead
		GM.instance.PlayerIsAlive = false;
		
		// Animate death
		if (withAnimation)
		{
			anim.SetTrigger ("DeathTrigger");
		}
        //GameObject.Instantiate(marioDieSound);
        Destroy(GameObject.Instantiate(marioDieSound), 4);

        yield return new WaitForSeconds(3f);
		// Wait some time before going to deathscreen
		//yield return new WaitForSeconds(withAnimation ? 2f : 1f);
		Application.LoadLevel (Application.loadedLevel);
	}
	
	void SetColliderSize()
	{
		if (curState == PlayerState.Small)
		{
			charCollider.size = new Vector2(0.16f, 0.16f);
			charCollider.offset = new Vector2(0f, 0.08f);
			headCollider.localPosition = new Vector3(0f, 0.14f);
		}
		else
		{
			charCollider.size = new Vector2(0.16f, 0.32f);
			charCollider.offset = new Vector2(0f, 0.16f);
			headCollider.localPosition = new Vector3(0f, 0.3f);
		}
	}
	
	void ToggleSpriteVisibility(bool fullVisible)
	{
		Color col = spriteRenderer.color;
		col.a = fullVisible ? 1f : 0.5f;
		spriteRenderer.color = col;
	}

	public void OnHeadHit(Collider2D other)
	{
		if (other.CompareTag (Tags.block))
		{
			BlockPowerup bPowerup = other.GetComponent<BlockPowerup>();
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
}

public enum PlayerState
{
	Small,
	Mushroom,
	Fireflower
}