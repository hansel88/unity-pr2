using UnityEngine;
using System.Collections;

// Level script for the level object that keeps track of the spawn point and whether the camera should move or not
public class Level : MonoBehaviour
{
	public Transform playerSpawnPoint; // Where the player will spawn in
	public bool staticCamera = false; // If true, the camera will spawn at 0, 0, 0 and not follow player, else it will spawn at the player
}