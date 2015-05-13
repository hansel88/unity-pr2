using UnityEngine;
using System.Collections;

public class HeadCollider : CollisionEntity
{
	void OnTriggerEnter2D(Collider2D other)
	{
		// TODO Slight cooldown for the collisions
		// if trigger collision happened first, wait .1 sec before doing anything in collision collision and vica versa
		/*if ((transform.parent.CompareTag (Tags.player) && other.CompareTag (Tags.block)))// || (transform.parent.CompareTag (Tags.enemy) && other.CompareTag (Tags.player)))
		{
			transform.parent.SendMessage ("OnHeadHit", other, SendMessageOptions.DontRequireReceiver);
		}*/
	}
}