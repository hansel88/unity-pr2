using UnityEngine;
using System.Collections;

public class EnemyShell : Enemy
{
	public int scoreMovingReward = 100;

	void Start()
	{
		// Start with no movement
		direction = 0;
	}

	[ContextMenu("Hit")]
	public void OnJumpHit()
	{
		JumpedOn ();
		// Check if shell is moving
		if (direction == 0) // Is not moving
		{
			// Reward player with score
			RewardScore ();
			// Start moving shell
			direction = 1;
		}
		else // Is moving
		{
			// Reward player with score
			RewardScore (scoreMovingReward);
			// Stop shell
			direction = 0;
		}
	}
}