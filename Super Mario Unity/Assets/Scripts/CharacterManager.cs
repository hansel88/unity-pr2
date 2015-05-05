using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
	public PlayerState curState;
	public bool hasStar = false;
	public bool isInvincible = false;

	public Transform spriteTranform;
	[HideInInspector]public Animator anim;
	private BoxCollider2D charCollider;
	private CharacterMovement charMove;
	private SpriteRenderer spriteRenderer;

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
		spriteRenderer = spriteTranform.GetComponent<SpriteRenderer>();
	}

	public void OnEnemyHit()
	{
		if (isInvincible)
		{
			return;
		}

		GM.instance.FreezeEntities ();

		anim.SetBool ("PoweringUp", false);
		anim.SetInteger ("PlayerState", (int)curState);
		anim.SetTrigger ("PowerupTrigger");
		isInvincible = true;
		switch(curState)
		{
		case PlayerState.Small:
			Die ();
			break;
		case PlayerState.Mushroom:
			curState = PlayerState.Small;
			// TODO To small
			break;
		case PlayerState.Fireflower:
			curState = PlayerState.Mushroom;
			// TODO To mushroom
			break;
		}
		SetColliderSize ();
	}

	public void PowerUpgrade(PlayerState toState)
	{
		if ((int)curState >= (int)toState)
		{
			// TODO Reward player
			GM.instance.Score += 1000;
			GUIManager.instance.PopRewardText (transform.position, "+" + 1000);
		}
		else
		{
			GM.instance.FreezeEntities ();
			curState = toState;
			anim.SetBool ("PoweringUp", true);
			anim.SetInteger ("PlayerState", (int)curState);
			anim.SetTrigger ("PowerupTrigger");
		}
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

	void SetColliderSize()
	{
		if (curState == PlayerState.Small)
		{
			charCollider.size = new Vector2(0.16f, 0.16f);
			charCollider.offset = new Vector2(0f, 0.08f);
		}
		else
		{
			charCollider.size = new Vector2(0.16f, 0.32f);
			charCollider.offset = new Vector2(0f, 0.16f);
		}
	}
}

public enum PlayerState
{
	Small,
	Mushroom,
	Fireflower
}