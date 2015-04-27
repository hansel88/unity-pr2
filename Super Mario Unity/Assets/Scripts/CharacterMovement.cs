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
        if ((Input.GetKeyDown("space") || Input.GetKeyDown("up")) && grounded == true)
        {
            grounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * speed * Time.deltaTime;
        }

	}

    void OnCollisionEnter2D(Collision2D colObj)
    {
        if (colObj.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

}
