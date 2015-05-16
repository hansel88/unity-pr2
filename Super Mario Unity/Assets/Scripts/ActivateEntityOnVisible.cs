using UnityEngine;
using System.Collections;

public class ActivateEntityOnVisible : MonoBehaviour
{
	private Entity entity;

	void Awake()
	{
		entity = transform.parent.GetComponent<Entity>();
	}

	void OnBecameVisible()
	{
		if (!entity.canMove)
			entity.canMove = true;
	}
}