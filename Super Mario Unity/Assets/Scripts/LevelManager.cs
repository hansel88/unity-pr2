using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{
	public Level[] levels;

	public static LevelManager instance;

	void Awake()
	{
		instance = this;
	}

	public void LoadLevel(int worldId, int levelId)
	{
		GM.instance.FreezeEntities ();
		//GM.instance.charManager.transform.position = levels[levelId].playerSpawnPoint.position;
		//Camera.main.transform.position = levels[levelId].cameraSpawnPoint.position;
	}

	public void OnStageStart()
	{
		GM.instance.UnFreezeEntities ();
		GM.instance.charManager.GetComponent<CharacterMovement>().canMove = true;
	}

	public void OnStageClear()
	{

	}
}