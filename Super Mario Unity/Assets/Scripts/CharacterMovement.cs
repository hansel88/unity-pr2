using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

	public float downForce = 10f;
    public float speed = 1.0f;
    public float jumpForce = 200.0f;
	public LayerMask groundedLayers;
	public Transform groundedPosition;
    public bool grounded = true;
	private Animator anim;
	private Rigidbody2D rBody;
	public Transform[] groundedChecks;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rBody = GetComponent<Rigidbody2D>();
	}
	float horizontalInput = 0f;
	void Update () 
	{
		// Grounded check
		Vector2 bl = groundedPosition.position - new Vector3(0.007f, 0.02f);
		Vector2 tr = groundedPosition.position + new Vector3(0.007f, 0.02f);
		grounded = Physics2D.OverlapArea (groundedChecks[0].position, groundedChecks[1].position, groundedLayers);

        //var move = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
		horizontalInput = Input.GetAxisRaw ("Horizontal");
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
			Jump ();
        }
        /*else
        {
            //GetComponent<Animator>().SetBool("WalkingRight", true); 
            //transform.position += move * speed * Time.deltaTime;
			//rBody.AddForce (move * speed * Time.deltaTime);

        }*/

        if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
		else if(horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);

		anim.SetFloat("movementSpeed", horizontalInput);
		anim.SetBool("Grounded", grounded);
	}

	void FixedUpdate()
	{
		float vel = rBody.velocity.y;
		if (!grounded)
		{
			vel -= downForce;
		}
		rBody.velocity = new Vector2(horizontalInput * speed, vel);
	}

	public void Jump()
	{
		if (!grounded) return;

        anim.SetTrigger("JumpTrigger");
		rBody.AddForce(new Vector2(0, jumpForce));
	}
}
