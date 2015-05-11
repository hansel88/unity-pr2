using UnityEngine;
using System.Collections;

public class FireflowerProjectile : MonoBehaviour
{
	[SerializeField]private Vector2 movementVelocity = new Vector2(2f, 2f);
	private Vector2 curVel;
	private Animator anim;
	private Rigidbody2D rBody;
	private float normalThreshold = 0.9f;
	private Vector2 savedVelocity;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rBody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Freeze/unfreeze fireball
		if (GM.instance.frozenEntities && !rBody.isKinematic)
		{
			savedVelocity = rBody.velocity;
			rBody.isKinematic = true;
		}
		else if (!GM.instance.frozenEntities && rBody.isKinematic)
		{
			rBody.velocity = savedVelocity;
			rBody.isKinematic = false;
		}
	}
	
	public void Initialize(bool moveRight)
	{
		movementVelocity.x *= moveRight ? 1 : -1;
		curVel = movementVelocity;
		rBody.velocity = curVel;

		Invoke ("Explode", 7f);
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		float normalX = other.contacts[0].normal.x;
		curVel = Vector2.zero;
		if (other.collider.CompareTag (Tags.enemy))
		{
			other.collider.SendMessage ("InstaDeath", 200, SendMessageOptions.DontRequireReceiver);
			Explode ();
		}
		else if (normalX > normalThreshold || normalX < -normalThreshold)
		{
			Explode ();
		}
		else if (!GM.instance.frozenEntities && !rBody.isKinematic)
		{
			curVel = movementVelocity;
		}
		rBody.velocity = curVel;
	}

	void Explode()
	{
		CancelInvoke ("Explode");
		GM.instance.charManager.DeductFireflowerCount ();
		anim.SetTrigger ("Explode");
		rBody.isKinematic = true;
		rBody.velocity = Vector2.zero;
	}
	
	public void DestroyEntity()
	{
		CancelInvoke ("Explode");
		// TODO Pool
		gameObject.SetActive (false);
	}
}