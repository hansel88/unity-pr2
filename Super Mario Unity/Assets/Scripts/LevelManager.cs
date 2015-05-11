using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{
	public static LevelManager instance;

	void Awake()
	{
		instance = this;
	}

	public void LoadLevel(int worldId, int levelId)
	{

	}

	public void OnStageStart()
	{

	}

	public void OnStageClear()
	{

	}
}