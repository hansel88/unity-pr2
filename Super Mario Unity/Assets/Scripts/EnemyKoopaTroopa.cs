using UnityEngine;
using System.Collections;

public class EnemyKoopaTroopa : Enemy
{
	public GameObject shellPrefab;

	[ContextMenu("Hit")]
	public void OnJumpHit()
	{
		Instantiate (shellPrefab, transform.position, Quaternion.identity);

		Die ();
	}

	public void OnCollide()
	{
		print ("Collided");
	}
}