using UnityEngine;
using System.Collections;

// Handling the pipe entrance point
public class PipeEntrance : MonoBehaviour
{
	public Level toLevel; // The level we are to be transported to
	public bool horizontalEntry = false; // To override the duck button (for horizontal pipes)

	private bool isEnteringPipe = false; // So we don't enter again while entering
	private CharacterManager charManager;

	void OnTriggerStay2D(Collider2D other)
	{
		// Check if we hit the player while pressing enter key and we aren't already entering
		if (other.CompareTag (Tags.player) && EnterKeyDown () && !isEnteringPipe)
		{
			// Get the charachermanager if we don't already have it
			if (!charManager)
			{
				charManager = other.GetComponent<CharacterManager>();
			}
			StartCoroutine (EnterPipe ());
		}
	}

	bool EnterKeyDown()
	{
		// If this is a horizontal entrypoint
		if (horizontalEntry)
		{
			// Return true if we are walking forwards
			return Input.GetAxis ("Horizontal") > 0.1f;
		}

		// Return true if duck is pressed
		return Input.GetButtonDown ("Duck");
	}

	IEnumerator EnterPipe()
	{
		isEnteringPipe = true;
		yield return StartCoroutine (charManager.AnimatePipeEntering ()); // Start and wait for the player to animatie into the pipe
		LevelManager.instance.LoadLevel (toLevel); // Load the new level
		isEnteringPipe = false;
	}
}