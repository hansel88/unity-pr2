using UnityEngine;
using System.Collections;

public class ModeSelect : MonoBehaviour {

    public int playerAmount;
    

	// Use this for initialization
	void Start () {
        playerAmount = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("down"))
        {
            playerAmount = 2;
            this.transform.position = new Vector3(-4, -1, 0);
        }

        if (Input.GetKey("up"))
        {
            playerAmount = 1;
            this.transform.position = new Vector3(-4, 0, 0);
        }

        if (Input.GetButtonDown("Submit"))
        {
            Application.LoadLevel("Loading_Level");
        }
	}
}
