using UnityEngine;
using System.Collections;

public class PipeScript : MonoBehaviour
{
	public string levelToLoad;
	private bool isEnteringPipe = false;
	private CharacterManager charManager;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag (Tags.player) && Input.GetButtonDown ("Duck") && !isEnteringPipe)
		{
			if (!charManager)
			{
				charManager = other.GetComponent<CharacterManager>();
			}
			StartCoroutine (EnterPipe ());
		}
	}

	IEnumerator EnterPipe()
	{
		isEnteringPipe = true;
		yield return StartCoroutine (charManager.AnimatePipeEntering ());
		Application.LoadLevel (levelToLoad);
	}
}