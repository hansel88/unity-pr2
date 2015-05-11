using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
	[HideInInspector]public Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OnActivate()
	{
		anim.SetTrigger ("ActivateTrigger");
	}
}