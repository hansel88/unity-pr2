using UnityEngine;
using System.Collections;

public class PlayerSpriteEvents : MonoBehaviour
{
	private CharacterMovement charMove;

	void Awake()
	{
		charMove = GetComponentInParent<CharacterMovement>();
	}

	public void PowerupDone()
	{
		GM.instance.UnFreezeEntities ();
		// TODO Resume game
	}
}