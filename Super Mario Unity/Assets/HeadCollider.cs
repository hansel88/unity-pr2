using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class HeadCollider : CollisionEntity
{
	void Awake()
	{
		gameObject.tag = Tags.headCollider;
		gameObject.layer = LayerMask.NameToLayer ("HeadCollider");
	}
}