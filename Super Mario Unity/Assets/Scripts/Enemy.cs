using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
	void Update()
	{
		HorizontalMovement ();
	}

	public void Die()
	{
		gameObject.SetActive (false); // TODO animate death and remove this
		GM.instance.Score += scoreReward;
	}
}