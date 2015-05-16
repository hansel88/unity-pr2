using UnityEngine;
using System.Collections;

public class StageEnd : MonoBehaviour
{
	public float slideSpeed = 1f;
	public Transform slideGoal;
	private Transform player;
	private bool isSliding = false;

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

		//player.GetComponent<CharacterMovement>().grounded = true;

		//player.position = Vector3.MoveTowards (player.position, new Vector3(player.position.x + 10f, player.position.y), slideSpeed * Time.deltaTime);
		//yield return new WaitForSeconds(3f);
		StartCoroutine (GM.instance.StageClear ());
	}
}