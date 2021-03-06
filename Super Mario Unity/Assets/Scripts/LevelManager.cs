﻿using UnityEngine;
using System.Collections;

// Handles loading levels
public class LevelManager : MonoBehaviour 
{
	public Level currentLevel; // The current level

	private Camera cam;
	private CameraController camController;
	public static LevelManager instance;

	void Awake()
	{
		instance = this;
		cam = Camera.main;
		camController = cam.GetComponent<CameraController>();
	}

	public void LoadLevel(Level toLevel)
	{
		// Deactivate the current level and activate the new level
		currentLevel.gameObject.SetActive (false);
		toLevel.gameObject.SetActive (true);
		currentLevel = toLevel;

		// Set the new cameraposition
		Vector3 camerPos = new Vector3(toLevel.playerSpawnPoint.transform.position.x, cam.transform.position.y, cam.transform.position.z);
		if (toLevel.staticCamera) // If static camera
		{
			// Keep the camera at the center
			camerPos = new Vector3(0f, 0f, cam.transform.position.z);
		}
		cam.transform.position = camerPos;
		camController.staticCamera = toLevel.staticCamera;

		// Set the playerpositio
		GM.instance.charManager.transform.position = toLevel.playerSpawnPoint.position;
	}
}