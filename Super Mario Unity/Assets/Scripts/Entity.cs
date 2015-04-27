﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
	public int scoreReward = 100;
	public float movementSpeed = 3f;
	public int direction = 1; // 1 = right, -1 = left, 0 = no movement
	public Transform spriteTransform; // The sprite to rotate when turning

	private bool isChangingDirection = false;

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
		// Moves the object in a horizontal direction
		transform.Translate ((transform.right * direction) * movementSpeed * Time.deltaTime);
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
				if (!isChangingDirection)
				{
					StartCoroutine (ChangeDirection ());
				}
			}
		}
	}
	
	IEnumerator ChangeDirection()
	{
		isChangingDirection = true;

		// Wait for end of frame to continue som we don't turn around while still colliding
		yield return new WaitForEndOfFrame();

		// Change direction
		direction *= -1;

		// Rotate spritetransform
		Vector3 scale = spriteTransform.localScale;
		scale.x *= -1;
		spriteTransform.localScale = scale;

		isChangingDirection = false;
	}

	public void RewardScore()
	{
		// Give the player scoreReward points
		GM.instance.Score += scoreReward;
	}
}