using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public float movementSpeed = 3f;
	public int direction = 1;
	public Transform spriteTransform; // The transform to rotate when turning

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
		yield return new WaitForEndOfFrame();
		direction *= -1;
		spriteTransform.localScale = new Vector3(direction, 1f);
		isChangingDirection = false;
	}
}