using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	public Transform playerSpawnPoint; // Where the player will spawn in
	public bool staticCamera = false; // If true, the camera will spawn at 0, 0, 0 and not follow player, else it will spawn at the player
}