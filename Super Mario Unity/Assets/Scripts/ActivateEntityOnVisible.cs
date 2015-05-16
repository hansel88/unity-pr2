using UnityEngine;
using System.Collections;

// Activates the parent entity when they become visible by the camera
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