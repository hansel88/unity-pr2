using UnityEngine;
using System.Collections;

// Script for the player sprite so functions can be called from the animation
public class PlayerSpriteEvents : MonoBehaviour
{
	// Called when the powerup is done (unfreezes the game)
	public void PowerupDone()
	{
		GM.instance.UnFreezeEntities ();
	}
}