using UnityEngine;
using System.Collections;

public class FireflowerProjectile : MonoBehaviour
{
	[SerializeField]private float movementForce = 10f;
	private Animator anim;
	private Rigidbody2D rBody;
	private Vector2 bounceVelocity;
	private bool saveBounceVelocity = true;
	private float normalThreshold = 0.9f;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rBody = GetComponent<Rigidbody2D>();
	}

	public void Initialize(bool moveRight)
	{
		rBody.AddForce ((moveRight ? Vector2.right : -Vector2.right) * movementForce);
		saveBounceVelocity = true;
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (saveBounceVelocity)
		{
			bounceVelocity = rBody.velocity;
			saveBounceVelocity = false;
		}
		Vector2 velocity = Vector2.zero;

		float normalX = other.contacts[0].normal.x;
		if (other.collider.CompareTag (Tags.enemy))
		{
			other.collider.SendMessage ("InstaDeath", SendMessageOptions.DontRequireReceiver);
			Explode ();
		}
		else if (normalX > normalThreshold || normalX < -normalThreshold)
		{
			Explode ();
		}
		else
		{
			velocity = new Vector2(rBody.velocity.x, bounceVelocity.y);
		}
		rBody.velocity = velocity;
	}
	
	void Explode()
	{
		anim.SetTrigger ("Explode");
		rBody.isKinematic = true;
		rBody.velocity = Vector2.zero;
	}
	
	public void DestroyEntity()
	{
		// TODO Pool
		gameObject.SetActive (false);
	}
}