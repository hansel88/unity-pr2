using UnityEngine;
using System.Collections;

public class PlayerSpriteEvents : MonoBehaviour
{
	private CharacterManager charManager;
	private CharacterMovement charMove;

	void Awake()
	{
		charManager = GetComponentInParent<CharacterManager>();
		charMove = GetComponentInParent<CharacterMovement>();
	}

	public void PowerupDone()
	{
		GM.instance.UnFreezeEntities ();
		// TODO Resume game
	}
}