using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	void Update()
	{
		HorizontalMovement ();
	}

	public void OnPlayerHit()
	{

	}

	public void OnDeath()
	{

	}
}