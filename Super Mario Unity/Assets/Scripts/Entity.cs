using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Entity : CollisionEntity
{
	public int scoreReward = 100;
	public float movementSpeed = 3f;
	public int direction = 1; // 1 = right, -1 = left, 0 = no movement
	public bool rotateWithDirection = true;
	public Transform spriteTransform; // The sprite to rotate when turning
	public bool canMove = true;
	private bool isChangingDirection = false;
	[HideInInspector]public Animator anim;
	private Rigidbody2D rbody;
	public bool isVisibleInCamera = true;

	public override void Awake()
	{
		base.Awake ();
		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		// Use the first child as a spritetransform incase nothing is assigned
		if (!spriteTransform)
		{
			Debug.LogWarning ("Assigning first child as spritetransform because none were assigned.", this);
			spriteTransform = transform.GetChild (0);
		}
	}

	public void HorizontalMovement()
	{
		if (!isVisibleInCamera) return;
		if (!GM.instance.frozenEntities && canMove)
		{
			if (rbody.isKinematic)
			{
				rbody.isKinematic = false;
			}
			// Moves the object in a horizontal direction
			transform.Translate ((transform.right * direction) * movementSpeed * Time.deltaTime);
		}
		else if (!rbody.isKinematic)
		{
			rbody.isKinematic = true;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.collider.CompareTag ("Ground"))
		{
			// Check if the collided object is on the correct side
			Vector3 otherPos = other.transform.position;
			Vector3 pos = transform.position;
			if ((otherPos.x > pos.x && direction == 1) || (otherPos.x < pos.x && direction == -1))
			{
				// Check if we are already changing direction before turning
				if (!isChangingDirection && gameObject.activeInHierarchy)
				{
					// TODO Check if the current collided object is the previous (that triggered the directionchange) 
					// that way it probably wont get stuck....
					StartCoroutine (ChangeDirection ());
				}
			}
		}
	}

	/*public virtual void OnCollide(Transform other)
	{
		print ("On entity collide");
		// Check if the collided object is on the correct side
		Vector3 otherPos = other.position;
		Vector3 pos = transform.position;
		if ((otherPos.x > pos.x && direction == 1) || (otherPos.x < pos.x && direction == -1))
		{
			print ("Change dir");
			// Check if we are already changing direction before turning
			if (!isChangingDirection && gameObject.activeInHierarchy)
			{
				StartCoroutine (ChangeDirection ());
			}
		}
	}*/
	
	IEnumerator ChangeDirection()
	{
		isChangingDirection = true;

		// Wait for end of frame to continue som we don't turn around while still colliding
		yield return new WaitForEndOfFrame();

		// Change direction
		direction *= -1;

		// Rotate spritetransform
		if (rotateWithDirection)
		{
			Vector3 scale = spriteTransform.localScale;
			scale.x *= -1;
			spriteTransform.localScale = scale;
		}

		isChangingDirection = false;
	}

	public void RewardScore(int score)
	{
		// Give the player score points
		GM.instance.Score += score;
		GUIManager.instance.PopRewardText (transform.position, "+" + score);
	}

	public void RewardScore()
	{
		RewardScore (scoreReward);
	}

	// TODO Remove
	public void JumpedOn()
	{
		hasBeenJumped = true;
		StartCoroutine (ResetJump ());
	}
	public bool hasBeenJumped = false;
	IEnumerator ResetJump()
	{
		yield return new WaitForEndOfFrame();
		hasBeenJumped = false;
	}
}