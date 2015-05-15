using UnityEngine;
using System.Collections;
using System.Timers;

public class CharacterMovement : MonoBehaviour {

	[SerializeField]private float downForce = 0.1f; // Force to apply when in the air
	[SerializeField]private float movementSpeed = 100f;
	[SerializeField]private float jumpForce = 250f;
	[SerializeField]private GameObject jumpSmall;
	[SerializeField]private GameObject jumpBig;

    public bool grounded = true;
	public bool canMove = true; // TODO Remove?
	private Animator anim;
	private Rigidbody2D rBody;
	[HideInInspector]public bool facingRight = true;
	private bool previousFacingRight = true; // TODO remove?
	private float horizontalInput = 0f;
    private bool jumpPressed = false;


	void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		// Get the animator for the player sprite
		anim = GetComponent<CharacterManager>().spriteTransform.GetComponent<Animator>();
	}

    void FixedUpdate()
    {
        if (jumpPressed)
            jumpForce += 5;
    }

	void Update () 
	{
		// Check if we can move
		if (canMove)
		{
			// Get input
			horizontalInput = Input.GetAxisRaw ("Horizontal");

			// Jumping
	        if (Input.GetButtonDown("Jump") && grounded == true)
	        {
				//Jump (false);
                jumpPressed = true;
	        }

            if( Input.GetButtonUp("Jump") && grounded == true)
            {
                Jump(false);
            }
	        
			// Flipping
			if (facingRight && horizontalInput < 0f)
			{
				Flip ();
			}
			else if (!facingRight && horizontalInput > 0f)
			{
				Flip ();
			}
	        
		}
		else 
		{
			horizontalInput = 0f;
		}

		// Set the animator parameters
		anim.SetBool ("Walking", horizontalInput < 0f || horizontalInput > 0f);
		anim.SetBool("Grounded", grounded);

		// Manage the y velocity
		float vel = rBody.velocity.y;
		if (!grounded)
		{
			// Add some downforce when in the air
			vel -= downForce;
		}

		// Check if the game is frozen
		if (!GM.instance.frozenEntities)
		{
			if (rBody.isKinematic)
			{
				rBody.isKinematic = false;
			}

			// Apply the input and the velocity to the rigidbody
			rBody.velocity = new Vector2(horizontalInput * movementSpeed * Time.deltaTime, vel);
		}
		else if (!rBody.isKinematic)
		{
			rBody.isKinematic = true;
		}
	}

	void Flip()
	{
		// TODO Remove??
		if ((previousFacingRight && !facingRight) || (!previousFacingRight && facingRight))
			anim.SetTrigger ("FlipTrigger");
		previousFacingRight = facingRight;

		// Flip the spirte
		facingRight = !facingRight;
		transform.localScale = new Vector3(facingRight ? 1 : -1, 1);
	}

	public void Jump(bool ignoreGrounded)
	{
        jumpPressed = false;
		// Check if we want to ignore the grounded value
		if (!ignoreGrounded)
			if (!grounded) return;

		// Play the jump SFX
		if (GM.instance.HasMushroom)
            Destroy(GameObject.Instantiate(jumpBig), 2);
        else if (jumpSmall != null)
            Destroy(GameObject.Instantiate(jumpSmall), 2);

		// Set us to not grounded and trigger the animtor
		//grounded = false;
        anim.SetTrigger("JumpTrigger");

		// Add the jumpforce to the rigidbody
		rBody.AddForce(new Vector2(0,Mathf.Clamp(jumpForce, 250f, 350f)));
        jumpForce = 250f;
        
	}

	void OnTriggerStay2D(Collider2D other)
	{
		// Check if we should be grounded
		if (ValidGroundTag (other.tag) && !grounded)
		{
			grounded = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		// Check if we shouldn't be grounded
		if (ValidGroundTag (other.tag))
		{
			grounded = false;
		}
	}

	bool ValidGroundTag(string groundTag)
	{
		// Checks if the tag is not any of the specified (A.K.A. we can be grounded)
		return !(groundTag.Equals (Tags.enemy) || groundTag.Equals (Tags.player) || groundTag.Equals (Tags.powerup));
	}
        
}
