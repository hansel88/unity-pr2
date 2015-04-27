using UnityEngine;
using System.Collections;

public class Powerup : Entity
{
	void Update()
	{
		HorizontalMovement ();
	}

	public virtual void OnPickup()
	{
		Destroy (gameObject);
	}
}