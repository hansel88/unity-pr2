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
	private bool isDying = false;

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

		SetAnimatioTriggers (false);
		isInvincible = true;

		switch(curState)
		{
			case PlayerState.Small:
				StartCoroutine (Die (true));
				break;
			case PlayerState.Mushroom:
				curState = PlayerState.Small;
				// TODO To small
				break;
			case PlayerState.Fireflower:
				curState = PlayerState.Small;
				// TODO To small
				break;
		}


		SetColliderSize ();

		if (!isDying)
		{
			StartCoroutine (FlashInvincible (3));
		}
	}

	public void PowerUpgrade(PlayerState toState)
	{
		if ((int)curState >= (int)toState)
		{
			int scoreReward = 1000;
			GM.instance.Score += scoreReward;
			GUIManager.instance.PopRewardText (transform.position, "+" + scoreReward);
		}
		else
		{
			GM.instance.FreezeEntities ();
			curState = toState;
			SetAnimatioTriggers (true);
		}
	}

	void SetAnimatioTriggers(bool poweringUp)
	{
		anim.SetBool ("PoweringUp", poweringUp);
		anim.SetInteger ("PlayerState", (int)curState);
		anim.SetTrigger ("PowerupTrigger");
	}

	IEnumerator FlashInvincible(int seconds)
	{
		float waitTime = 0.2f; // Time to wait between flashes

		// Flashing loop
		for (int i = 0; i < seconds / waitTime; i ++)
		{
			ToggleSpriteVisibility (i % 2 == 0);
			yield return new WaitForSeconds(0.2f);
		}

		// Make sure player is fully visible when finished with the invincibility
		ToggleSpriteVisibility (true);

		isInvincible = false;
	}
	
	public IEnumerator Die(bool withAnimation)
	{
		// If we are already dying, don't die again
		if (isDying) yield return null;

		// Set that we are dying so we can't double die
		isDying = true;

		// Disable charactercollider
		charCollider.enabled = false;

		// Tell GM we are dead
		GM.instance.PlayerIsAlive = false;

		// Animate death
		if (withAnimation)
		{
			anim.SetTrigger ("DeathTrigger");
		}

		// Wait some time before going to deathscreen
		yield return new WaitForSeconds(withAnimation ? 2f : 1f);
		Application.LoadLevel (Application.loadedLevel);
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

	void ToggleSpriteVisibility(bool fullVisible)
	{
		Color col = spriteRenderer.color;
		col.a = fullVisible ? 1f : 0.5f;
		spriteRenderer.color = col;
	}
}

public enum PlayerState
{
	Small,
	Mushroom,
	Fireflower
}