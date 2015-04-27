using UnityEngine;
using System.Collections;

public class EnemyGoomba : Enemy
{
	public void OnJumpHit()
	{
		Die ();
	}
}