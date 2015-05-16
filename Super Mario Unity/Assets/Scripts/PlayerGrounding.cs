using UnityEngine;
using System.Collections;

public class PlayerGrounding : MonoBehaviour
{
	private CharacterMovement charMove;
	// TODO Remove
	void Awake()
	{
		charMove = GetComponentInParent<CharacterMovement>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//charMove.grounded = true;
	}
}