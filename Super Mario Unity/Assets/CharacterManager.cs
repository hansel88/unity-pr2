using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
	public Transform spriteTranform;
	[HideInInspector]public Animator anim;
	private BoxCollider2D charCollider;

	void Awake()
	{
		if (!spriteTranform)
		{
			Debug.LogWarning ("No spritetransform assigned to the player!", this);
			Debug.Break ();
		}
		charCollider = GetComponent<BoxCollider2D>();
		anim = spriteTranform.GetComponent<Animator>();
	}

	public void OnDeath()
	{
		print ("death");
		charCollider.enabled = false;
		GM.instance.playerIsAlive = false;
		anim.SetTrigger ("DeathTrigger");

	}
}