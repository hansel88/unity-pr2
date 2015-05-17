using UnityEngine;
using System.Collections;

// Handling the flagpole events
public class StageEnd : MonoBehaviour
{
	public float slideSpeed = 1f;
	public Transform slideGoal; // Where to slide to
	private Transform player;
	private bool isSliding = false;
	private int[] scores = new int[]{400, 1200, 2500, 5000}; // Scores rewarded depending on the height

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		// Activate sliding if we hit the player
		if (other.CompareTag (Tags.player) && !isSliding)
		{
			StartCoroutine (StartSlide ());
		}
	}

	IEnumerator StartSlide()
	{
		yield return new WaitForEndOfFrame();

		// Don't slide again if we are already sliding
		if (isSliding) yield return null;
		isSliding = true;

		// Stop player from moving
		player.GetComponent<CharacterMovement>().canMove = false;
		player.GetComponent<Rigidbody2D>().isKinematic = true;

		// Determing the score reward depening on where the player hit the pole
		int index = 0;
		float dist = Vector2.Distance (player.position, new Vector2(player.position.x, slideGoal.position.y));
		if (dist < 0.016f)
		{
			index = 0;
		}
		else if (dist < 0.038f)
		{
			index = 1;
		}
		else if (dist < 0.064f)
		{
			index = 2;
		}
		else if (dist >= 0.064f)
		{
			index = 3;
		}
		int rewardScore = scores[index];

		// Slide player
		while (player.position.y > slideGoal.position.y)
		{
			player.position = Vector3.MoveTowards (player.position, new Vector3(player.position.x, slideGoal.position.y), slideSpeed * Time.deltaTime);
			yield return null;
		}

		// Reward the player
		GM.instance.Score += rewardScore;
		GUIManager.instance.PopRewardText (player.position, "" + rewardScore);

		// Start the stage clear method
		StartCoroutine (GM.instance.StageClear ());
	}
}