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
		other.SendMessage ("OnDeath", SendMessageOptions.DontRequireReceiver);
		//base.OnCollide (other);
	}
}