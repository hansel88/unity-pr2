using UnityEngine;
using System.Collections;

// The fireflower projectiles movement and collision
public class FireflowerProjectile : MonoBehaviour
{
	[SerializeField]private Vector2 movementVelocity = new Vector2(2f, 2f); // The movement vector
	private Vector2 curVel; // Current movement vector
	private Animator anim;
	private Rigidbody2D rBody;
	private float normalThreshold = 0.9f; // Threshold for the normal when hitting
	private Vector2 savedVelocity;
	private bool hasExploded = false;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rBody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Freeze/unfreeze fireball
		if (GM.instance.frozenEntities && !rBody.isKinematic)
		{
			savedVelocity = rBody.velocity;
			rBody.isKinematic = true;
		}
		else if (!GM.instance.frozenEntities && rBody.isKinematic)
		{
			rBody.velocity = savedVelocity;
			rBody.isKinematic = false;
		}
	}
	
	public void Initialize(bool moveRight)
	{
		// Start moving the projectile
		movementVelocity.x *= moveRight ? 1 : -1;
		curVel = movementVelocity;
		rBody.velocity = curVel;

		// Explode in 7 seconds
		Invoke ("Explode", 7f);
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		// Get the normal
		float normalX = other.contacts[0].normal.x;

		curVel = Vector2.zero;
		if (!hasExploded)
		{
			if (other.collider.CompareTag (Tags.enemy)) // If hit enemy
			{
				// Fire kill event in enemey
				other.collider.SendMessage ("InstaDeath", 200, SendMessageOptions.DontRequireReceiver);
				Explode ();
			}
			else if (normalX > normalThreshold || normalX < -normalThreshold) // If hit vertical wall
			{
				// Stop projectile
				Explode ();
			}
			else if (!GM.instance.frozenEntities && !rBody.isKinematic)
			{
				// Unfreeze
				curVel = movementVelocity;
			}
		}
		rBody.velocity = curVel;
	}

	void Explode()
	{
		// Prevent it to explode again
		if (hasExploded) return;
		hasExploded = true;

		GM.instance.charManager.fireflowerCount --;

		anim.SetTrigger ("Explode");
		rBody.gravityScale = 0f;
	}
	
	public void DestroyEntity()
	{
		CancelInvoke ("Explode");
		// TODO Pool
		gameObject.SetActive (false);
	}
}