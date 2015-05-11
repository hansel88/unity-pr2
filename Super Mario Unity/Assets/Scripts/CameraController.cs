﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform Player;        // Reference to the player's transform.
	public Transform centerTransform;
	public Transform[] edgeColliders;

    void Awake()
    {
        //Setting up the reference.
        //Player = GameObject.FindGameObjectWithTag("Player").transform; 
        Player = GameObject.FindWithTag("Player").transform;
    }

	void Start()
	{
		float screenHeight = Vector2.Distance (new Vector2(0f, GM.instance.camWorldBottomLeft.y), 
		                                       new Vector2(0f, GM.instance.camWorldTopRight.y));
		SetEdgeCollider (edgeColliders[0], 
		                 new Vector3(GM.instance.camWorldBottomLeft.x - 0.5f, centerTransform.position.y, 0f), 
		                 new Vector2(1f, screenHeight));
		SetEdgeCollider (edgeColliders[1], 
		                 new Vector3(GM.instance.camWorldTopRight.x + 0.5f, centerTransform.position.y, 0f), 
		                 new Vector2(1f, screenHeight));
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
        TrackPlayer();
    }

    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.

        //GM.instance.UpdateScreenBounds();
		float targetX = Player.position.x;

		if(Player.position.x > centerTransform.position.x)
        {
            this.transform.position = new Vector3(targetX, this.transform.position.y, transform.position.z);
        }
        //float targetY = Player.position.y;

        // Set the camera's position to the target position with the same z component.

    }
}
