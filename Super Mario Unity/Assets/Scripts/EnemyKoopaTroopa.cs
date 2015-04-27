using UnityEngine;
using System.Collections;

public class EnemyKoopaTroopa : Enemy
{
	public GameObject shellPrefab;

	public void OnJumpHit()
	{
		Instantiate (shellPrefab, transform.position, Quaternion.identity);

		Die ();
	}
}