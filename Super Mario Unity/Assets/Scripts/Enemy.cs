using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	public bool jump = false;
	public float jumpForce = 100f;
	public float jumpRate = 1f;
	private float curJumpTimer;
	private Rigidbody2D rBody;

	void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		HorizontalMovement ();

		if (jump)
		{
			curJumpTimer += Time.deltaTime;
			if (curJumpTimer >= jumpRate)
			{
				rBody.AddForce (Vector2.up * jumpForce);
				curJumpTimer = 0f;
			}
		}
	}
}