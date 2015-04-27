using UnityEngine;
using System.Collections;

public class EnemyKoopaTroopa : Enemy
{
	public GameObject shellPrefab;

	public void OnJumpHit()
	{
		GameObject shell = Instantiate (shellPrefab, transform.position, Quaternion.identity) as GameObject;
		//shell.GetComponent<EnemyShell>().direction = 0;

		Die ();
	}
}