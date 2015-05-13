using UnityEngine;
using System.Collections;

// TODO Delete
public class Block : MonoBehaviour
{
	[HideInInspector]public Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OnActivate()
	{
		if (anim)
			anim.SetTrigger ("ActivateTrigger");
	}
}