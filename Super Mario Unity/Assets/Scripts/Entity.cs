using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public int scoreReward = 100;
	public float movementSpeed = 3f;
	public int direction = 1; // 1 = right, -1 = left, 0 = no movement
	public bool rotateWithDirection = true;
	public Transform spriteTransform; // The sprite to rotate when turning
	public bool canMove = true;
	private bool isChangingDirection = false;
	[HideInInspector]public Animator anim;
	[HideInInspector]public Rigidbody2D rBody;
	public bool isVisibleInCamera = true;
	[HideInInspector]public BoxCollider2D boxCollider;

	void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	public virtual void Start()
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
		if (!GM.instance.frozenEntities && canMove)
		{
			if (rBody.isKinematic)
			{
				if (anim)
				{
					anim.enabled = true;
				}
				rBody.isKinematic = false;
			}
			// Moves the object in a horizontal direction
			transform.Translate ((Vector3.right * direction) * movementSpeed * Time.deltaTime);
		}
		else if (GM.instance.frozenEntities && !rBody.isKinematic)
		{
			if (anim)
			{
				anim.enabled = false;
			}
			rBody.isKinematic = true;
		}
	}

	public void ChangeDirectionOnCollision(Collision2D other)
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
					StartCoroutine (TurnAround ());
				}
			}
		}
	}
	
	public virtual IEnumerator TurnAround()
	{
		isChangingDirection = true;

		// Change direction
		direction *= -1;

		// Rotate spritetransform
		if (rotateWithDirection)
		{
			Vector3 scale = spriteTransform.localScale;
			scale.x *= -1;
			spriteTransform.localScale = scale;
		}

		// Wait for end of frame to continue som we don't turn around while still colliding
		yield return new WaitForEndOfFrame();

		isChangingDirection = false;
	}

	public void RewardScore(int score)
	{
		// Give the player score points
		GM.instance.Score += score;
		GUIManager.instance.PopRewardText (transform.position, score.ToString ());
	}

	public void RewardScore()
	{
		RewardScore (scoreReward);
	}

	public virtual void OnHeadHit(Collider2D other)
	{
		print ("Hit head collider");
	}
}