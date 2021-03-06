﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To make sure the camera is assigned to the screen space canvas
public class RewardCanvas : MonoBehaviour
{
	void Awake()
	{
		// Make sure the reward canvas' camera is assigned
		Canvas canvas = GetComponent<Canvas>();
		if (canvas.worldCamera == null)
		{
			canvas.worldCamera = Camera.main;
		}
	}
}