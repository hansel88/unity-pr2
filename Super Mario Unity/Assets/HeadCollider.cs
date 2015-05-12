using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class HeadCollider : CollisionEntity
{
	void OnTriggerEnter2D(Collider2D other)
	{
		transform.parent.SendMessage ("OnHeadHit", other, SendMessageOptions.DontRequireReceiver);
	}
}