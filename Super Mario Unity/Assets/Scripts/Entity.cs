using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public float movementSpeed = 10f;
	public int direction = 1;

	public void Movement()
	{
		transform.Translate ((transform.right * movementSpeed * Time.deltaTime) * direction);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag ("Ground"))
		{
			direction *= -1;
		}
	}
}