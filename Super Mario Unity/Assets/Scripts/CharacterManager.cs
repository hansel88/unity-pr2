using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
	public Transform spriteTranform;
	[HideInInspector]public Animator anim;
	private BoxCollider2D charCollider;
	private CharacterMovement charMove;

	void Awake()
	{
		if (!spriteTranform)
		{
			Debug.LogWarning ("No spritetransform assigned to the player!", this);
			Debug.Break ();
		}
		charCollider = GetComponent<BoxCollider2D>();
		charMove = GetComponent<CharacterMovement>();
		anim = spriteTranform.GetComponent<Animator>();
	}

	public void OnEnemyHit()
	{
		if (GM.instance.CharacterStatus == GM.MarioStatus.Small)
		{
			Die ();
		}
		PowerDowngrade ();
	}

	void Die()
	{
		charCollider.enabled = false;
		GM.instance.PlayerIsAlive = false;
		anim.SetTrigger ("DeathTrigger");

	}

	public void PowerUpgrade()
	{
		GM.instance.FreezeEntities ();
		if (GM.instance.CharacterStatus == GM.MarioStatus.Small && GM.instance.HasMushroom)
		{
			GM.instance.CharacterStatus = GM.MarioStatus.Big;
			anim.SetBool ("Mushroom", true);
			anim.SetBool ("IsSmall", true);
			anim.SetTrigger ("PowerupTrigger");
			charCollider.size = new Vector2(0.16f, 0.32f);
			charCollider.offset = new Vector2(0f, 0.16f);
		}
	}

	public void PowerDowngrade()
	{
		// TODO Make player invincible???

		if (GM.instance.CharacterStatus == GM.MarioStatus.Big && GM.instance.HasMushroom)
		{
			GM.instance.CharacterStatus = GM.MarioStatus.Small;
			anim.SetBool ("Mushroom", false);
			anim.SetBool ("IsSmall", false);
			anim.SetTrigger ("PowerupTrigger");
			charCollider.size = new Vector2(0.16f, 0.16f);
			charCollider.offset = new Vector2(0f, 0.08f);
		}
	}
}