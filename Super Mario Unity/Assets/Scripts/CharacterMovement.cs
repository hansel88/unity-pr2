using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float downForce = 10f;
    public float speed = 1.0f;
    public float jumpForce = 200.0f;
	public LayerMask groundedLayers;
	public Transform groundedPosition;
    public bool grounded = true;
	public bool canMove = true;
	private Animator anim;
	private Rigidbody2D rBody;
	public bool facingRight = true;
	public Transform[] groundedChecks;
	private bool previousFacingRight = true;
	private float cameraEdgeMargin = 0.09f;

    public GameObject jumpSmall;
    public GameObject jumpBig;

	void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		anim = GetComponent<CharacterManager>().anim;
	}

	float horizontalInput = 0f;
	void Update () 
	{
		// Grounded check
		Vector2 bl = groundedPosition.position - new Vector3(0.007f, 0.02f);
		Vector2 tr = groundedPosition.position + new Vector3(0.007f, 0.02f);
		//grounded = Physics2D.OverlapArea (groundedChecks[0].position, groundedChecks[1].position, groundedLayers);

		if (canMove)
		{
	        //var move = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
			horizontalInput = Input.GetAxisRaw ("Horizontal");
	        if (Input.GetButtonDown("Jump") && grounded == true)
	        {
				Jump (false);
	        }
	        /*else
	        {
	            //GetComponent<Animator>().SetBool("WalkingRight", true); 
	            //transform.position += move * speed * Time.deltaTime;
				//rBody.AddForce (move * speed * Time.deltaTime);

	        }*/
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

		// Clamp position to camera
		/*float xPos = transform.position.x;
		if (xPos < GM.instance.camWorldBottomLeft.x + cameraEdgeMargin)
		{
			horizontalInput = Mathf.Clamp (horizontalInput, 0f, 1f);
		}
		else if (xPos > GM.instance.camWorldTopRight.x - cameraEdgeMargin)
		{
			horizontalInput = Mathf.Clamp (horizontalInput, -1f, 0f);
		}*/

		anim.SetBool ("Walking", horizontalInput < 0f || horizontalInput > 0f);
		//anim.SetFloat("movementSpeed", h);
		anim.SetBool("Grounded", grounded);

		float vel = rBody.velocity.y;
		if (!grounded)
		{
			vel -= downForce;
		}
		if (!GM.instance.frozenEntities)
		{
			if (rBody.isKinematic)
			{
				rBody.isKinematic = false;
			}
			rBody.velocity = new Vector2(horizontalInput * speed * Time.deltaTime, vel);
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

		facingRight = !facingRight;

		transform.localScale = new Vector3(facingRight ? 1 : -1, 1);
	}
	
	void FixedUpdate()
	{

	}

	public void Jump(bool ignoreGrounded)
	{
		if (!ignoreGrounded)
			if (!grounded) return;

        if (GM.instance.HasMushroom)
            Destroy(GameObject.Instantiate(jumpBig), 2);
        else if (jumpSmall != null)
            GameObject.Instantiate(jumpSmall);

		grounded = false;
        anim.SetTrigger("JumpTrigger");
		rBody.AddForce(new Vector2(0, jumpForce));
	}
}
