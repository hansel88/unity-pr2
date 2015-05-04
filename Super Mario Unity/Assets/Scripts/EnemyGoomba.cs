using UnityEngine;
using System.Collections;

public class EnemyGoomba : Enemy
{
	public void OnJumpHit()
	{
		Die ();
	}

	public void OnCollide(Transform other)
	{
		other.SendMessage ("OnEnemyHit", SendMessageOptions.DontRequireReceiver);
		//base.OnCollide (other);
	}
}