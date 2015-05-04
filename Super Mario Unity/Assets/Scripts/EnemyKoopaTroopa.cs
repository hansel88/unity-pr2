using UnityEngine;
using System.Collections;

public class EnemyKoopaTroopa : Enemy
{
	public GameObject shellPrefab;

	[ContextMenu("Hit")]
	public void OnJumpHit()
	{
		JumpedOn ();

		Instantiate (shellPrefab, transform.position, Quaternion.identity);

		Die ();
	}

	public void OnCollide(Transform other)
	{
		other.SendMessage ("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
		//base.OnCollide (other);
	}
}