using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
	public Transform spriteTranform;
	[HideInInspector]public Animator anim;

	void Awake()
	{
		if (!spriteTranform)
		{
			Debug.LogWarning ("No spritetransform assigned to the player!", this);
			Debug.Break ();
		}
		anim = spriteTranform.GetComponent<Animator>();
	}

	public void OnDeath()
	{
		print ("death");
		GM.instance.playerIsAlive = false;
		anim.SetTrigger ("DeathTrigger");
	}
}