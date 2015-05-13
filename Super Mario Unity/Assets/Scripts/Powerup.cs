using UnityEngine;
using System.Collections;

public class Powerup : Entity
{
	public bool isActive = true;

	void Update()
	{
		HorizontalMovement ();
	}

	public virtual void OnPickup()
	{
		gameObject.SetActive (false);
	}
}