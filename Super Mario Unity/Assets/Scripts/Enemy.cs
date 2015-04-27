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
		GM.instance.Score += scoreReward;
		gameObject.SetActive (false); // TODO animate death and remove this
	}
}