using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform Player;        // Reference to the player's transform.

    void Awake()
    {
        //Setting up the reference.
        //Player = GameObject.FindGameObjectWithTag("Player").transform; 
        Player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = Player.position.x;
        //float targetY = Player.position.y;

        // Set the camera's position to the target position with the same z component.
        this.transform.position = new Vector3(targetX, this.transform.position.y, transform.position.z);
    }
}
