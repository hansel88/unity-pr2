﻿using UnityEngine;
using System.Collections;

// Handles powerup movement and deactivation
public class Powerup : Entity
{
	public bool isActive = true;

	void Update()
	{
		HorizontalMovement ();
	}

	public virtual void OnPickup()
	{
		gameObject.SetActive (false); // TODO Pool
	}
}