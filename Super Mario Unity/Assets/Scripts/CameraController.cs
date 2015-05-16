using UnityEngine;
using System.Collections;

// Controller for the camera following the player
public class CameraController : MonoBehaviour {

    public Transform Player;        // Reference to the player's transform.
	public Transform centerTransform; // Tranform for the center of the camera
	public Transform[] edgeColliders; // Colliders for limiting the player inside the camera view
	public Transform entityActivatorCollider; // The collider for actiating the entities when hitting them
	public bool staticCamera = false; // Whether the camera should move or not

    void Awake()
    {
        //Setting up the reference.
        Player = GameObject.FindWithTag("Player").transform;
    }

	void Start()
	{
		// position the colliders
		float screenHeight = Vector2.Distance (new Vector2(0f, GM.instance.camWorldBottomLeft.y), 
		                                       new Vector2(0f, GM.instance.camWorldTopRight.y));
		SetEdgeCollider (edgeColliders[0], 
		                 new Vector3(GM.instance.camWorldBottomLeft.x - 0.5f, centerTransform.position.y, 0f), 
		                 new Vector2(1f, screenHeight));
		SetEdgeCollider (edgeColliders[1], 
		                 new Vector3(GM.instance.camWorldTopRight.x + 0.5f, centerTransform.position.y, 0f), 
		                 new Vector2(1f, screenHeight));
		SetEdgeCollider (entityActivatorCollider, edgeColliders[1].position, edgeColliders[1].GetComponent<BoxCollider2D>().size);
	}

	void SetEdgeCollider(Transform collider, Vector3 pos, Vector3 size)
	{
		collider.SetParent (null);
		collider.position = pos;
		collider.GetComponent<BoxCollider2D>().size = size;
		collider.SetParent (transform);
	}

    void FixedUpdate()
    {
		if (!staticCamera)
        	TrackPlayer();
    }

    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.

		float targetX = Player.position.x;

		if(Player.position.x > centerTransform.position.x)
        {
            this.transform.position = new Vector3(targetX, this.transform.position.y, transform.position.z);
        }

        // Set the camera's position to the target position with the same z component.

    }
}
