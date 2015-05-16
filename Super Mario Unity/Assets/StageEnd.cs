using UnityEngine;
using System.Collections;

public class StageEnd : MonoBehaviour
{
	public float slideSpeed = 1f;
	public Transform slideGoal;
	private Transform player;
	private bool isSliding = false;
	private int[] scores = new int[]{400, 1200, 2500, 5000};

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag (Tags.player) && !isSliding)
		{
			StartCoroutine (StartSlide ());
		}
	}

	IEnumerator StartSlide()
	{
		yield return new WaitForEndOfFrame();
		if (isSliding) yield return null;
		isSliding = true;

		player.GetComponent<CharacterMovement>().canMove = false;
		player.GetComponent<Rigidbody2D>().isKinematic = true;

		while (player.position.y > slideGoal.position.y)
		{
			//lerpTime += Time.deltaTime;
			//player.position = Vector3.Lerp (player.position, new Vector3(player.position.x, slideGoal.position.y), lerpTime / curDuration);
			player.position = Vector3.MoveTowards (player.position, new Vector3(player.position.x, slideGoal.position.y), slideSpeed * Time.deltaTime);
			yield return null;
		}
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
		GM.instance.Score += rewardScore;
		GUIManager.instance.PopRewardText (player.position, "" + rewardScore);

		//player.GetComponent<CharacterMovement>().grounded = true;

		//player.position = Vector3.MoveTowards (player.position, new Vector3(player.position.x + 10f, player.position.y), slideSpeed * Time.deltaTime);
		//yield return new WaitForSeconds(3f);
		StartCoroutine (GM.instance.StageClear ());
	}
}