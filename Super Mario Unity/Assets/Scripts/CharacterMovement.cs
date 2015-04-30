using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {


    float speed = 1.0f;
    float jumpForce = 200.0f;
    private bool grounded = true;

	// Use this for initialization
	void Start () {
	    
	}


	
	// Update is called once per frame
	void Update () {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if ((Input.GetKeyDown("space") || Input.GetKeyDown("up")) && grounded == true)
        {
            GetComponent<Animator>().SetTrigger("JumpTrigger");
            grounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else
        {
            //GetComponent<Animator>().SetBool("WalkingRight", true); 
            if( !(move.x < 0 && transform.position.x <= GM.instance.camWorldBottomLeft.x)) //Prevent Mario from walking out of the left side of the screen
                transform.position += move * speed * Time.deltaTime;
        }
        if (move.x < 0)
            GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
        else if(move.x > 0)
            GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        GetComponent<Animator>().SetFloat("movementSpeed", move.x);
	}

    void OnCollisionEnter2D(Collision2D colObj)
    {
        if (colObj.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

}
